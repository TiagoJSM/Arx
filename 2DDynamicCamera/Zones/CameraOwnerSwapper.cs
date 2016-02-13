using CommonInterfaces.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace _2DDynamicCamera.Zones
{
    public class CameraOwnerSwapper : MonoBehaviour
    {
        void OnTriggerEnter2D(Collider2D other)
        {
            var playerControl = other.GetComponent<IPlayerControl>();
            if(playerControl == null)
            {
                return;
            }
            DynamicCamera.Main.Owner = other.transform;
        }
    }
}
