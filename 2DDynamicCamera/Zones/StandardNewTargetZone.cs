using _2DDynamicCamera.Zones.Target;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace _2DDynamicCamera.Zones
{
    public enum ColliderType
    {
        Box, 
        Circle
    }

    public class StandardNewTargetZone : NewTargetZone
    {
        private ColliderType _selectedColliderType;
        private Collider2D _collider;

        public ColliderType colliderType;

        protected override Collider2D Collider { get { return _collider; } }

        void Start()
        {
            var colliders = this.gameObject.GetComponents<Collider2D>();
            foreach (var collider in colliders)
            {
                DestroyImmediate(collider);
            }
            _selectedColliderType = colliderType;
            SetCollider(_selectedColliderType);
        }

        void Update()
        {
            if (_selectedColliderType != colliderType)
            {
                _selectedColliderType = colliderType;
                SetCollider(_selectedColliderType);
            }
        }

        protected override void OnCameraOwnerEnter()
        {
            DynamicCamera.Main.AddTarget(this);
        }

        protected override void OnCameraOwnerExit()
        {
            DynamicCamera.Main.RemoveTarget(this);
        }

        private void SetCollider(ColliderType _selectedColliderType)
        {
            switch (_selectedColliderType)
            {
                case ColliderType.Box:
                    SetBoxCollider();
                    break;
                case ColliderType.Circle:
                    SetCircleCollider();
                    break;
            }
        }

        private void SetBoxCollider()
        {
            RemoveCollider();
            AddCollider<BoxCollider2D>();
        }

        private void SetCircleCollider()
        {
            RemoveCollider();
            AddCollider<CircleCollider2D>();
        }

        private void AddCollider<TCollider>() where TCollider : Collider2D
        {
            _collider = this.gameObject.AddComponent<CircleCollider2D>();
            _collider.isTrigger = true;
        }

        private void RemoveCollider()
        {
            if (_collider != null)
            {
                DestroyImmediate(_collider);
            }
        }
    }
}
