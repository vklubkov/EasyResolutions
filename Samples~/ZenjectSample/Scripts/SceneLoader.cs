using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

namespace EasyResolutions.ZenjectSample {
    public class SceneLoader : MonoBehaviour {
        [SerializeField] string _sceneName;
        [SerializeField] Button _nextButton;

        IEasyResolutionsPicker _easyResolutionsPicker;

        [Inject]
        void Construct(IEasyResolutionsPicker easyResolutionsPicker) => _easyResolutionsPicker = easyResolutionsPicker;

        void Start() {
            var sceneName = _easyResolutionsPicker.PickResolution(_sceneName);
            _nextButton.onClick.AddListener(() => SceneManager.LoadScene(sceneName));
        }
    }
}