using Assets.Standard_Assets.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Standard_Assets._2DDynamicCamera.Zones
{
    public abstract class BaseZone : MonoBehaviour
    {
        private bool _ownerIsInside = false;

        void OnTriggerEnter2D(Collider2D other)
        {
            if (_ownerIsInside)
            {
                return;
            }
            var dynamicCamera = DynamicCamera.Main;
            if (dynamicCamera == null)
            {
                return;
            }
            if (dynamicCamera.Owner == null)
            {
                return;
            }
            if (other.gameObject.IsPlayer())
            {
                _ownerIsInside = true;
                OnCameraOwnerEnter();
            }
        }

        void OnTriggerExit2D(Collider2D other)
        {
            if (!_ownerIsInside)
            {
                return;
            }
            var dynamicCamera = DynamicCamera.Main;
            if (dynamicCamera == null)
            {
                return;
            }
            if (dynamicCamera.Owner == null)
            {
                return;
            }
            if (other.gameObject.IsPlayer())
            {
                _ownerIsInside = false;
                OnCameraOwnerExit();
            }
        }

        protected abstract void OnCameraOwnerEnter();
        protected abstract void OnCameraOwnerExit();
    }
}
