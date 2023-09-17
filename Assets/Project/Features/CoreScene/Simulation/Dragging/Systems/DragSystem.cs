using Leopotam.EcsLite;
using Project.Features.CoreScene.Simulation.Camera.Components;
using Project.Features.CoreScene.Simulation.Camera.Components.Tags;
using Project.Features.CoreScene.Simulation.Dragging.Components;
using Project.Features.CoreScene.Simulation.Dragging.Settings;
using Project.Features.CoreScene.Simulation.Transforms.Components;
using UnityEngine;
using UnityEngine.Assertions;

namespace Project.Features.CoreScene.Simulation.Dragging.Systems
{
    public class DragSystem 
        : IEcsInitSystem
        , IEcsRunSystem
    {
        private readonly MovableSettings _movableSettings;
        
        private EcsWorld _world;

        private EcsPool<TransformRefComponent> _transformComponentPool;
        private EcsPool<CameraRefComponent> _cameraComponentPool;

        private EcsFilter _movableFilter;
        private EcsFilter _cameraFilter;

        public DragSystem(MovableSettings movableSettings)
        {
            Assert.IsNotNull(movableSettings);
            
            _movableSettings = movableSettings;
        }

        public void Init(IEcsSystems systems)
        {
            _world = systems
                .GetWorld();

            _movableFilter = _world
                .Filter<DraggedTag>()
                .Inc<TransformRefComponent>()
                .End();

            _cameraFilter = _world
                .Filter<CameraRefComponent>()
                .Inc<CurrentCameraTag>()
                .End();
            
            _transformComponentPool = _world
                .GetPool<TransformRefComponent>();
            
            _cameraComponentPool = _world
                .GetPool<CameraRefComponent>();
        }

        public void Run(IEcsSystems systems)
        {
            if (_cameraFilter.GetEntitiesCount() <= 0)
            {
                return;
            }
            var cameraEntity = _cameraFilter.GetRawEntities()[0];

            ref var cameraComponent = ref _cameraComponentPool
                .Get(cameraEntity);
            
            foreach (var entity in _movableFilter)
            {
                ref var transformComponent = ref _transformComponentPool
                    .Get(entity);
                
                var screenPointRay = cameraComponent.Ref
                    .ScreenPointToRay(Input.mousePosition);

                if (!Physics.Raycast(screenPointRay, out var hit, float.MaxValue, _movableSettings.SlotsMask))
                {
                    continue;
                }
                transformComponent.Ref.position = hit.point;
            }
        }
    }
}