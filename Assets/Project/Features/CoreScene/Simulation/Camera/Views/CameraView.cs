using UnityEngine;

namespace Project.Features.CoreScene.Simulation.Camera.Views
{
    public class CameraView
        : MonoBehaviour
    {
        [SerializeField] 
        private UnityEngine.Camera _camera;
        
        public UnityEngine.Camera Camera => _camera;
    }
}