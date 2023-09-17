using UnityEngine;

namespace Project.Features.LoadingResourcesScene.UI.Popups.LoadingResourcesThrobberPopup
{
    public class LoadingResourcesThrobberPopup
        : MonoBehaviour
    {
        [SerializeField] 
        private RectTransform _throbberWidget;

        [SerializeField] 
        private float _rotationSpeed;

        private void Update()
        {
            _throbberWidget.Rotate(Vector3.back, _rotationSpeed * Time.deltaTime);
        }
    }
}