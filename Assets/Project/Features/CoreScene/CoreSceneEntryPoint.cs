using Project.Features.CoreScene.Simulation;
using UnityEngine;
using UnityEngine.Assertions;

namespace Project.Features.CoreScene
{
    public class CoreSceneEntryPoint
        : MonoBehaviour
    {
        [SerializeField] 
        private CoreSceneSimulationRunner _simulationRunner;

        private void Awake()
        {
            Assert.IsNotNull(_simulationRunner);
        }

        private void Start()
        {
            _simulationRunner.Initialize();
            _simulationRunner.Run();
        }
    }
}