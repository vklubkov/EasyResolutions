using UnityEngine;
using Zenject;

namespace EasyResolutions.ZenjectSample {
    [CreateAssetMenu(fileName = "EasyResolutionsInstaller", menuName = "Installers/EasyResolutionsInstaller")]
    public class EasyResolutionsInstaller : ScriptableObjectInstaller<EasyResolutionsInstaller> {
        public override void InstallBindings() =>
            Container
                .BindInterfacesTo<EasyResolutionsPickerAdapter>()
                .AsSingle();
    }
}