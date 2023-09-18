using Leopotam.EcsLite;
using Project.Features.CoreScene.Simulation.Camera.Systems;
using Project.Features.CoreScene.Simulation.Cells.Systems;
using Project.Features.CoreScene.Simulation.Dragging.Systems;
using UnityEngine;
using UnityEngine.Assertions;

namespace Project.Features.CoreScene.Simulation
{
    public class CoreSceneSimulationRunner
        : MonoBehaviour
    {
        [SerializeField] 
        private CoreSceneDependencies _dependencies;        
        
        private bool _isRunning;
        private bool _isInitialized;

        private EcsWorld _defaultWorld;
        
        private EcsSystems _updateSystems;
        private EcsSystems _fixedUpdateSystems;

        private void Awake()
        {
            Assert.IsNotNull(_dependencies);
        }

        private void Update()
        {
            if (!_isRunning)
            {
                return;
            }
            _updateSystems.Run();
        }

        private void LateUpdate()
        {
            if (!_isRunning)
            {
                return;
            }
        }

        private void FixedUpdate()
        {
            if (!_isRunning)
            {
                return;
            }
            _fixedUpdateSystems.Run();
        }

        private void OnDestroy()
        {
            _defaultWorld?.Destroy();
            _updateSystems?.Destroy();
        }

        public void Initialize()
        {
            Assert.IsFalse(_isInitialized);

            _defaultWorld = new EcsWorld();
            
            _updateSystems = new EcsSystems(_defaultWorld);
            
            _updateSystems
#if UNITY_EDITOR
                // Регистрируем отладочные системы по контролю за состоянием каждого отдельного мира:
                // .Add (new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem ("events"))
                .Add (new Leopotam.EcsLite.UnityEditor.EcsWorldDebugSystem ())
                // Регистрируем отладочные системы по контролю за текущей группой систем. 
                .Add (new Leopotam.EcsLite.UnityEditor.EcsSystemsDebugSystem ())
#endif
                .Add(new CellsInitializeSystem(_dependencies.CellsGridView))
                .Add(new CameraInitializationSystem(_dependencies.CameraView))
                .Add(new DraggedEntitiesSpawnerSystem(_dependencies.DraggingSettings))
                .Add(new DragEndSystem(_dependencies.DraggingSettings))
                .Init();

            _fixedUpdateSystems = new EcsSystems(_defaultWorld);
            
            _fixedUpdateSystems
                .Add(new DragSystem(_dependencies.DraggingSettings))
#if UNITY_EDITOR
                .Add(new Leopotam.EcsLite.UnityEditor.EcsSystemsDebugSystem ())
#endif
                .Init();
            
            _isInitialized = true;
        }

        public void Run()
        {
            Assert.IsTrue(_isInitialized);

            _isRunning = true;
        }
    }
}