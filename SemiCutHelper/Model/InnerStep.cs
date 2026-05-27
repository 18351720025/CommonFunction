using SemiCutHelper.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace SemiCutHelper.Model
{
    public class InnerStep
    {
        public int Count { get; set; }

        public double Step { get; set; }

        public double CutSpeed { get; set; }

        public ICutProcessParameters? CutLineParameter { get; set; }
    }
}
