using UnityEngine;

namespace EasyResolutions.AdvancedSample {
    [CreateAssetMenu(fileName = "EasyResolutions_CustomComparer", menuName = "Easy Resolutions/Custom Comparer")]
    public class CustomComparer : EasyResolutionsComparer {
        public override double Epsilon { protected get; set; } = 0.008;
        public override (int Width, int Height) Target { protected get; set; }

        public override int Compare(IResolution a, IResolution b) {
            if (a == null && b == null)
                return 0;

            if (b == null)
                return -1;

            if (a == null)
                return 1;

            if (a.Width == b.Width && a.Height == b.Height)
                return 0;

            if (a.Width == Target.Width && a.Height == Target.Height)
                return -1;

            if (b.Width == Target.Width && b.Height == Target.Height)
                return 1;

            return 0;
        }
    }
}