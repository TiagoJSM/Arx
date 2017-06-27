using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Assets.Standard_Assets.Extensions;

namespace Assets.Standard_Assets.Environment.Platforms.Gate.Scripts
{
    public class GateOpener : MonoBehaviour
    {
        [SerializeField]
        private GateBehaviour _gate;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.IsPlayer())
            {
                _gate.Open();
            }
        }
    }
}
