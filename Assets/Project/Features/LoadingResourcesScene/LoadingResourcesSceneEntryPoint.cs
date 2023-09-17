using Project.Features.Global.ResourcesLoader;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Project.Features.LoadingResourcesScene
{
    public class LoadingResourcesSceneEntryPoint
        : MonoBehaviour
    {
        [SerializeField] 
        private int CoreSceneIndex = 1;
        
        private ResourcesLoader _resourcesLoader;
        
        private void Awake()
        {
            _resourcesLoader = new ResourcesLoader();
        }

        private async void Start()
        {
            await _resourcesLoader.LoadResource();

            SceneManager.LoadScene(CoreSceneIndex);
        }
    }
}