using Project.Features.CoreScene.Simulation.Camera.Views;
using Project.Features.CoreScene.Simulation.Cells.Views;
using Project.Features.CoreScene.Simulation.Dragging.Settings;
using UnityEngine;

namespace Project.Features.CoreScene
{
    public class CoreSceneDependencies
        : MonoBehaviour
    {
        [SerializeField] 
        private MovableSettings _movableSettings;
        
        [SerializeField] 
        private CellsGridView _cellsGridView;

        [SerializeField]
        private CameraView _cameraView;

        public CellsGridView CellsGridView 
            => _cellsGridView;

        public CameraView CameraView
            => _cameraView;
    
        public MovableSettings MovableSettings 
            => _movableSettings;
    }
}