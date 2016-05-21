using _2DDynamicCamera.Interfaces;
using DevelopmentUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Extensions;

namespace _2DDynamicCamera.Zones.Target
{
    public abstract class NewTargetZone : BaseZone, ICameraTarget
    {
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

        void OnDrawGizmos()
        {
            DrawTarget();
        }

        private void DrawTarget()
        {
            DrawArrow.ForGizmo(Center.ToVector2(), relativeTarget, Color.red);
        }
    }
}
