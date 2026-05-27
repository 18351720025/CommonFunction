using System;
using System.Collections.Generic;
using System.Text;

namespace SemiCutHelper.Interface
{
    public interface ICutProcessParameters
    {
        bool Validate(out string errorMessage);
    }
}
