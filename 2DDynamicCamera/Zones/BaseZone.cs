using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace _2DDynamicCamera.Zones
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
            if (dynamicCamera.owner == null)
            {
                return;
            }
            if (other.gameObject == dynamicCamera.owner.gameObject)
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
            if (dynamicCamera.owner == null)
            {
                return;
            }
            if (other.gameObject == dynamicCamera.owner.gameObject)
            {
                _ownerIsInside = false;
                OnCameraOwnerExit();
            }
        }

        protected abstract void OnCameraOwnerEnter();
        protected abstract void OnCameraOwnerExit();
    }
}
