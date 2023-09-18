using Leopotam.EcsLite;
using Project.Features.CoreScene.Simulation.Camera.Components;
using Project.Features.CoreScene.Simulation.Camera.Components.Tags;
using Project.Features.CoreScene.Simulation.Cells.Components;
using Project.Features.CoreScene.Simulation.Cells.Components.Tags;
using Project.Features.CoreScene.Simulation.Cells.Views;
using Project.Features.CoreScene.Simulation.Dragging.Components;
using Project.Features.CoreScene.Simulation.Dragging.Settings;
using Project.Features.CoreScene.Simulation.Transforms.Components;
using UnityEngine;
using UnityEngine.Assertions;

namespace Project.Features.CoreScene.Simulation.Dragging.Systems
{
    public class DragEndSystem
        : IEcsInitSystem
        , IEcsRunSystem
    {
        private const int ClickInputButtonId = 0;
        
        private readonly DraggingSettings _settings;
        
        private EcsWorld _world;

        private EcsFilter _draggedObjectsFilter;
        private EcsFilter _currentCamerasFilter;

        private EcsPool<TransformRefComponent> _transformRefComponentPool;
        private EcsPool<CellOccupiedTag> _cellOccupiedTagPool;
        private EcsPool<CameraRefComponent> _cameraPool;
        private EcsPool<DraggedTag> _draggedTagPool;
        
        public DragEndSystem(DraggingSettings settings)
        {
            Assert.IsNotNull(settings);
            
            _settings = settings;
        }

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();

            _transformRefComponentPool = _world
                .GetPool<TransformRefComponent>();
            
            _cellOccupiedTagPool = _world
                .GetPool<CellOccupiedTag>();

            _draggedTagPool = _world
                .GetPool<DraggedTag>();
            
            _cameraPool = _world
                .GetPool<CameraRefComponent>();
            
            _draggedObjectsFilter = _world
                .Filter<DraggedTag>()
                .Inc<TransformRefComponent>()
                .End();

            _currentCamerasFilter = _world
                .Filter<CameraRefComponent>()
                .Inc<CurrentCameraTag>()
                .End();
        }

        public void Run(IEcsSystems systems)
        {
            if (!Input.GetMouseButton(ClickInputButtonId))
                return;

            if (_currentCamerasFilter.GetEntitiesCount() <= 0)
                return;

            var cameraEntity = _currentCamerasFilter
                .GetRawEntities()[0];

            ref var cameraComponent = ref _cameraPool
                .Get(cameraEntity);

            if (cameraComponent.Ref == null)
                return;

            var screenRay = cameraComponent.Ref
                .ScreenPointToRay(Input.mousePosition);
            
            if (!Physics.Raycast(screenRay, out var hit, float.MaxValue, _settings.SlotsMask))
                return;

            var view = hit.transform
                .GetComponentInParent<CellView>();

            if (view == null)
                return;

            if (!view.BindedEntity.Unpack(_world, out var cellEntity))
                return;

            if (_cellOccupiedTagPool.Has(cellEntity))
                return;

            if (_draggedObjectsFilter.GetEntitiesCount() <= 0)
                return;

            var draggedObjectEntity = _draggedObjectsFilter
                .GetRawEntities()[0];
            
            ref var cellTransformRefComponent = ref _transformRefComponentPool
                .Get(cellEntity);
            
            ref var draggedEntityTransformRefComponent = ref _transformRefComponentPool
                .Get(draggedObjectEntity);

            draggedEntityTransformRefComponent.Ref.position = cellTransformRefComponent.Ref.position;

            _cellOccupiedTagPool.Add(cellEntity);
            _draggedTagPool.Del(draggedObjectEntity);
        }
    }
}