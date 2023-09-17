using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace Project.Features.CoreScene.Simulation.Cells.Views
{
    public class CellsGridView
        : MonoBehaviour
    {
        [Serializable]
        public class CellsRow
        {
            [SerializeField]
            private List<CellView> _cells = new();
            
#if UNITY_EDITOR
            public List<CellView> Cells
            {
                get => _cells;
                set => _cells = value;
            }
#else
            public IReadOnlyList<CellView> Cells => _cells;
#endif
        }

        [SerializeField]
        private List<CellsRow> _cellsRows = new();

#if UNITY_EDITOR
        public List<CellsRow> CellsRows
        {
            get => _cellsRows;
            set => _cellsRows = value;
        }
#else
        public IReadOnlyList<CellsRow> CellsRows => _cellsRows;
#endif
        private void Awake()
        {
            Assert.IsNotNull(_cellsRows);
        }
    }
}