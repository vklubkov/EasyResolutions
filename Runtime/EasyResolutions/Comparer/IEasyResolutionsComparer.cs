using System.Collections.Generic;

namespace EasyResolutions {
    public interface IEasyResolutionsComparer : IComparer<IResolution> {
        public double Epsilon { set; }
        public (int Width, int Height) Target { set; }
    }
}