using Leopotam.EcsLite;
using UnityEngine;

namespace Project.Features.CoreScene.Simulation.Cells.Views
{
    public class CellView
        : MonoBehaviour
    {
        [SerializeField] 
        private Vector2 _position;

        public EcsPackedEntity BindedEntity
        {
            get;
            private set;
        }

        public Vector2 Position
        {
            get => _position;
#if UNITY_EDITOR
            set => _position = value;
#endif
        }

        public void Initialize(EcsPackedEntity entity)
        {
            BindedEntity = entity;
        }
    }
}