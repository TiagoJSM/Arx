using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Standard_Assets.Environment.Treadmill.Scripts
{
    public class Treadmill : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {
            var crate = collision.gameObject.GetComponent<TreadmillCrate>();
            if(crate != null)
            {
                crate.MoveOnPath();
            }
        }
    }
}
