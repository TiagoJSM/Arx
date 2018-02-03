using DevelopmentUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Extensions;
using Assets.Standard_Assets._2DDynamicCamera.Interfaces;

namespace Assets.Standard_Assets._2DDynamicCamera.Zones.Target
{
    public abstract class NewTargetZone : BaseZone, ICameraTarget
    {
        [SerializeField]
        private bool _useTargetDamping = true;
        [SerializeField]
        private Vector2 _damping = Vector2.one;
        public Vector2 relativeTarget = new Vector2(0, 1);

        public Vector3 Position
        {
            get 
            {
                return Center + relativeTarget.ToVector3(); 
            }
            set
            {
                relativeTarget = (value - Center).ToVector2();
            }
        }

        //protected abstract Collider2D Collider { get; }

        private Vector3 Center
        {
            get
            {
                return transform.position;
            }
        }

        public Vector2? Damping
        {
            get
            {
                if (_useTargetDamping)
                {
                    return _damping;
                }
                return null;
            }
        }

        void OnDrawGizmos()
        {
            DrawTarget();
        }

        private void DrawTarget()
        {
            DrawArrow.ForGizmo(Center.ToVector2(), relativeTarget, Color.red, 1f, 40f);
        }
    }
}
