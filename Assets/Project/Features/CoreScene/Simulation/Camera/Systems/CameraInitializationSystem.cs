using Leopotam.EcsLite;
using Project.Features.CoreScene.Simulation.Camera.Components;
using Project.Features.CoreScene.Simulation.Camera.Components.Tags;
using Project.Features.CoreScene.Simulation.Camera.Views;
using UnityEngine.Assertions;

namespace Project.Features.CoreScene.Simulation.Camera.Systems
{
    public class CameraInitializationSystem 
        : IEcsInitSystem
    {
        private readonly CameraView _cameraView;

        private EcsWorld _world;

        private EcsPool<CameraRefComponent> _cameraComponentPool;
        private EcsPool<CurrentCameraTag> _currentCameraTagPool;

        public CameraInitializationSystem(CameraView cameraView)
        {
            Assert.IsNotNull(cameraView);
            
            _cameraView = cameraView;
        }

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();

            _cameraComponentPool = _world
                .GetPool<CameraRefComponent>();
            
            _currentCameraTagPool = _world
                .GetPool<CurrentCameraTag>();

            var entity = _world.NewEntity();

            ref var cameraComponent = ref _cameraComponentPool.Add(entity);

            cameraComponent.Ref = _cameraView.Camera;
            
            _currentCameraTagPool.Add(entity);
        }
    }
}