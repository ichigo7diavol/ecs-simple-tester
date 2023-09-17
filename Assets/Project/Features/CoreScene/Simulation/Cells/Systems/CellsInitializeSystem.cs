using Leopotam.EcsLite;
using Project.Features.CoreScene.Simulation.Cells.Components;
using Project.Features.CoreScene.Simulation.Cells.Views;
using Project.Features.CoreScene.Simulation.Transforms.Components;
using UnityEngine;
using UnityEngine.Assertions;

namespace Project.Features.CoreScene.Simulation.Cells.Systems
{
    public class CellsInitializeSystem 
        : IEcsInitSystem
    {
        private readonly CellsGridView _cellsGridView;
        
        private EcsWorld _world;
        
        private EcsPool<CellViewRefComponent> _cellViewComponentPool;
        private EcsPool<CellComponent> _cellComponentPool;
        
        private EcsPool<TransformRefComponent> _transformComponentPool;
        
        public CellsInitializeSystem(CellsGridView cellsGridView)
        {
            Assert.IsNotNull(cellsGridView);
            
            _cellsGridView = cellsGridView;
        }

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();

            _cellViewComponentPool = _world.GetPool<CellViewRefComponent>();
            _cellComponentPool = _world.GetPool<CellComponent>();
            _transformComponentPool = _world.GetPool<TransformRefComponent>();
            
            for (var rowIndex = 0; rowIndex < _cellsGridView.CellsRows.Count; rowIndex++)
            {
                var row = _cellsGridView.CellsRows[rowIndex];

                for (var columnIndex = 0; columnIndex < row.Cells.Count; columnIndex++)
                {
                    var cell = row.Cells[columnIndex];

                    var entity = _world.NewEntity();

                    cell.Initialize(_world.PackEntity(entity));
                    
                    ref var cellComponent = ref _cellComponentPool.Add(entity);

                    cellComponent.GridPosition = new Vector2(columnIndex, rowIndex);

                    ref var cellViewComponent = ref _cellViewComponentPool.Add(entity);

                    cellViewComponent.Ref = cell;
                    
                    ref var transformComponent = ref _transformComponentPool.Add(entity);

                    transformComponent.Ref = cell.transform;
                }
            }
        }
    }
}