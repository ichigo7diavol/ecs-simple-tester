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
        private DraggingSettings _draggingSettings;
        
        [SerializeField] 
        private CellsGridView _cellsGridView;

        [SerializeField]
        private CameraView _cameraView;

        public CellsGridView CellsGridView 
            => _cellsGridView;

        public CameraView CameraView
            => _cameraView;
    
        public DraggingSettings DraggingSettings 
            => _draggingSettings;
    }
}