using SemiCutHelper.Model.Enums;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace SemiCutHelper.Model
{
    /// <summary>
    /// Represents the geometric and identification information for a semiconductor wafer, including type, dimensions,
    /// orientation, and center position.
    /// </summary>
    /// <remarks>Use this structure to encapsulate all relevant details about a wafer's physical
    /// characteristics and classification. This type is typically used in applications involving wafer handling,
    /// inspection, or processing where precise geometric and type information is required.</remarks>
    public struct WaferInfo
    {
        /// <summary>
        /// 获取或设置晶圆的类型。
        /// </summary>
        public WaferType WaferType { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the entity includes a flat component.
        /// </summary>
        public bool HasFlat { get; set; }

        /// <summary>
        /// Gets or sets the flat length value.
        /// </summary>
        public double FlatLength { get; set; }

        /// <summary>
        /// Gets or sets the direction projected onto a flat (2D) plane, ignoring any vertical component.
        /// </summary>
        public Direction FlatDirection { get; set; }

        /// <summary>
        /// Gets or sets the center point of the shape.
        /// </summary>
        public Point Center { get; set; }

        public double Width { get; set; }

        public double Height { get; set; }

        public double Angle { get; set; }
    }

    public struct  CutRegionExt
    {
        public double XStartExt { get; set; }

        public double XEndExt { get; set; }

        public double YExt { get; set; }
    }
}
