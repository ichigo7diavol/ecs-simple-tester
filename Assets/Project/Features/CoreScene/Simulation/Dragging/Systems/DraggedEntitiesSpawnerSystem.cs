using Leopotam.EcsLite;
using Project.Features.CoreScene.Simulation.Dragging.Components;
using Project.Features.CoreScene.Simulation.Dragging.Settings;
using Project.Features.CoreScene.Simulation.Transforms.Components;
using UnityEngine;
using UnityEngine.Assertions;

namespace Project.Features.CoreScene.Simulation.Dragging.Systems
{
    public class DraggedEntitiesSpawnerSystem
        : IEcsInitSystem, IEcsRunSystem
    {
        private readonly DraggingSettings _settings;

        private EcsWorld _world;

        private EcsFilter _draggedEntityFilter;
        
        private EcsPool<DraggedTag> _draggedTagsPool;
        private EcsPool<TransformRefComponent> _transformRefComponentsPool;

        public DraggedEntitiesSpawnerSystem(DraggingSettings settings)
        {
            Assert.IsNotNull(settings);
            
            _settings = settings;
        }

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();

            _draggedEntityFilter = _world
                .Filter<DraggedTag>()
                .Inc<TransformRefComponent>()
                .End();
            
            _draggedTagsPool = _world
                .GetPool<DraggedTag>();
            
            _transformRefComponentsPool = _world
                .GetPool<TransformRefComponent>();
        }

        public void Run(IEcsSystems systems)
        {
            if (_draggedEntityFilter.GetEntitiesCount() > 0)
            {
                return;
            }
            var instance = Object.Instantiate(_settings.DraggedObjectPrefab);

            var entity = _world.NewEntity();

            _draggedTagsPool.Add(entity);

            ref var transformComponent = ref _transformRefComponentsPool.Add(entity);

            transformComponent.Ref = instance.transform;
        }
    }
}