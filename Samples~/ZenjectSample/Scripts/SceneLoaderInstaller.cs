using UnityEngine;
using Zenject;

namespace EasyResolutions.ZenjectSample {
    [CreateAssetMenu(fileName = "SceneLoaderInstaller", menuName = "Installers/SceneLoaderInstaller")]
    public class SceneLoaderInstaller : ScriptableObjectInstaller<SceneLoaderInstaller> {
        [SerializeField] SceneLoader _sceneLoader;

        public override void InstallBindings() =>
            Container
                .Bind<SceneLoader>()
                .FromComponentInNewPrefab(_sceneLoader)
                .AsSingle()
                .NonLazy();
    }
}