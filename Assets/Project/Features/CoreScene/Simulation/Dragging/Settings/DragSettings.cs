using UnityEngine;

namespace Project.Features.CoreScene.Simulation.Dragging.Settings
{
    [CreateAssetMenu(menuName = "Settings/" + nameof(DraggingSettings))]
    public class DraggingSettings : ScriptableObject
    {
        [SerializeField] 
        private GameObject _draggedObjectPregab;

        [SerializeField] 
        private LayerMask _slotsMask;

        public GameObject DraggedObjectPrefab 
            => _draggedObjectPregab;
        
        public LayerMask SlotsMask 
            => _slotsMask;
    }
}