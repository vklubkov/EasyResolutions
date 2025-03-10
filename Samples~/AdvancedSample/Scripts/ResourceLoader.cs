using UnityEngine;

namespace EasyResolutions.AdvancedSample {
    public class ResourceLoader : MonoBehaviour  {
        void Start() {
            var canvasResource = "Canvas".PickResolution("Resources");
            var preloadedCanvas = Resources.Load<Canvas>(canvasResource);
            Instantiate(preloadedCanvas);
        }
    }
}