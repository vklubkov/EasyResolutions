using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace EasyResolutions.AdvancedSample {
    public class SceneLoader : MonoBehaviour {
        [SerializeField] string _sceneName;
        [SerializeField] Button _nextButton;
        void Start() => _nextButton.onClick.AddListener(() => SceneManager.LoadScene(_sceneName));
    }
}