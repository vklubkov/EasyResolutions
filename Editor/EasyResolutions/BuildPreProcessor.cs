using UnityEditor.Build;

namespace EasyResolutions {
    internal class BuildPreProcessor : BuildPlayerProcessor {
        public override int callbackOrder {
            get {
                var easyResolutionsSettings = EasyResolutions_Settings.Instance;
                if (easyResolutionsSettings == null)
                    return 0;

                return easyResolutionsSettings.BuildPlayerProcessorOrder;
            }
        }

        public override void PrepareForBuild(BuildPlayerContext _) => EditorDataProvider.Update();
    }
}