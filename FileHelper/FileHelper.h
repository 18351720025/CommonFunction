#pragma once
#include <windows.h>
#include <iostream>
// 7.0.0 版本 Header-only 引入
#include <msgpack.hpp>


    // --- 2. 内存布局定义 ---
#pragma pack(push, 1) // 确保字节对齐一致
struct MmfLayout {
    uint32_t data_size;      // 记录当前有效数据的字节数
    char raw_data[1024 * 1024 * 64]; // 预留 64MB 数据区
};
#pragma pack(pop)

template <typename  T> class JFileHelper
{
public:
    JFileHelper(const char* path);

    ~JFileHelper();

    // 写入数据
    bool Save(const T& data);

    // 读取数据
    bool Load(T& outData);

private:
    HANDLE hFile = INVALID_HANDLE_VALUE;
    HANDLE hMapping = INVALID_HANDLE_VALUE;
    void* pView = nullptr;
    MmfLayout* layout = nullptr;
};


template<typename  T> JFileHelper<T>::JFileHelper(const char* path)
{
    // 创建/打开文件
    hFile = CreateFileA(path, GENERIC_READ | GENERIC_WRITE, FILE_SHARE_READ, NULL,
        OPEN_ALWAYS, FILE_ATTRIBUTE_NORMAL, NULL);

    // 确保文件大小足够容纳我们的布局
    DWORD lowSize = sizeof(MmfLayout);
    hMapping = CreateFileMapping(hFile, NULL, PAGE_READWRITE, 0, lowSize, NULL);
    pView = MapViewOfFile(hMapping, FILE_MAP_ALL_ACCESS, 0, 0, lowSize);

    layout = static_cast<MmfLayout*>(pView);
}

template<typename  T> JFileHelper<T>::~JFileHelper()
{
    if (pView) UnmapViewOfFile(pView);
    if (hMapping) CloseHandle(hMapping);
    if (hFile) CloseHandle(hFile);
}

template<typename  T> bool JFileHelper<T>::Save(const T& data)
{
    try {
        // 序列化
        msgpack::sbuffer buffer;
        msgpack::pack(buffer, data);

        if (buffer.size() > sizeof(layout->raw_data)) return false;
        layout->data_size = 0;
        // 拷贝数据
        memcpy(layout->raw_data, buffer.data(), buffer.size());

        // 更新长度头 (这一步决定了“有效性”)
        // 只要这一行执行完，即使进程立刻崩溃，数据也是安全的
        layout->data_size = static_cast<uint32_t>(buffer.size());

        return true;
    }
    catch (...) {
        return false;
    }
}

template<typename  T> bool JFileHelper<T>::Load(T& outData)
{
    if (layout->data_size == 0) return false;

    try {
        // 7.0.0 反序列化逻辑
        msgpack::object_handle oh = msgpack::unpack(layout->raw_data, layout->data_size);
        oh.get().convert(outData);
        return true;
    }
    catch (const std::exception& e) {
        std::cerr << "解包失败: " << e.what() << std::endl;
        return false;
    }
}



