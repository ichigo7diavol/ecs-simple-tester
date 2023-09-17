using System.Collections.Generic;
using NUnit.Framework;
using Project.Features.CoreScene.Simulation.Cells.Views;
using UnityEditor;
using UnityEditor.EditorTools;
using UnityEditor.SceneManagement;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace Project.Features.CoreScene.Simulation.Cells.Editor.Tools
{
    [EditorTool("Cells grid creator", typeof(CellsGridView))]
    public class CellGridViewEditorTool : EditorTool
    {
        private VisualElement _rootElement;
        
        private CellsGridView _target;

        private ObjectField _firstPrefabView;
        private ObjectField _secondPrefabView;
        private Vector2IntField _gridSizeField;
        private FloatField _cellSizeField;
        
        public override void OnActivated()
        {
            base.OnActivated();
            
            _rootElement = new VisualElement
            {
                style =
                {
                    position = Position.Absolute,
                    left = 0.0f,
                    bottom = 0.0f,
                    backgroundColor = new StyleColor(Color.black),
                    width = new StyleLength(Length.Percent(40))
                }
            };
            _firstPrefabView = new ObjectField("First cell view")
            {
                objectType = typeof(CellView)
            };
            _secondPrefabView = new ObjectField("Second cell view")
            {
                objectType = typeof(CellView)
            };
            _gridSizeField = new Vector2IntField("Grid size field");
            _cellSizeField = new FloatField("Cell size");

            var generateButton = new Button(GenerateGrid)
            {
                text = "Generate"
            };
            _rootElement.Add(_firstPrefabView);
            _rootElement.Add(_secondPrefabView);
            _rootElement.Add(_gridSizeField);
            _rootElement.Add(_cellSizeField);
            _rootElement.Add(generateButton);
            
            var sceneView = SceneView.lastActiveSceneView;
            
            sceneView.rootVisualElement.Add(_rootElement);

            _target = Selection
                .gameObjects[0]
                .GetComponent<CellsGridView>();
        }

        public override void OnWillBeDeactivated()
        {
            base.OnWillBeDeactivated();
            
            _rootElement?.RemoveFromHierarchy();
        }
        
        private void GenerateGrid()
        {
            Assert.IsNotNull(_firstPrefabView.value as CellView);
            Assert.IsNotNull(_secondPrefabView.value as CellView);
            Assert.IsNotNull(_target);
            
            while (_target.transform.childCount > 0)
            {
                DestroyImmediate(_target.transform.GetChild(0).gameObject);
            }
            _target.CellsRows ??= new List<CellsGridView.CellsRow>();

            _target.CellsRows.Clear();
            
            var gridSize = (Vector2) _gridSizeField.value * _cellSizeField.value / 2;
            var startPoint = _target.transform.position 
                             - new Vector3(gridSize.x, 0.0f, gridSize.y)
                             + new Vector3(1.0f, 0.0f, 1.0f) * _cellSizeField.value / 2.0f;
            
            for (var rowIndex = 0; rowIndex < _gridSizeField.value.y; rowIndex++)
            {
                var row = new CellsGridView.CellsRow();

                _target.CellsRows.Add(row);

                var isRowEven = rowIndex % 2 == 0;
                
                for (var columnIndex = 0; columnIndex < _gridSizeField.value.x; columnIndex++)
                {
                    var isColumnEven = columnIndex % 2 == 0;
                    
                    var prefab = isColumnEven == isRowEven 
                        ? _firstPrefabView.value as CellView 
                        : _secondPrefabView.value as CellView;

                    var position = startPoint 
                                   + Vector3.forward * rowIndex * _cellSizeField.value 
                                   + Vector3.right * columnIndex * _cellSizeField.value;

                    var cell = (CellView) PrefabUtility.InstantiatePrefab(prefab, _target.transform);

                    cell.Position = new Vector2(columnIndex, rowIndex);
                    
                    cell.transform.position = position;

                    cell.gameObject.name = $"Cell [{columnIndex}; {rowIndex}]";
                    
                    row.Cells.Add(cell);
                }
            }

            EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
        }
    }
}