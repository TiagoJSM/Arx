using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Standard_Assets.Scripts
{
    public class SetGameObjectActive : MonoBehaviour
    {
        private bool _done;

        public GameObject target;
        public bool onlyOnce;
        public bool valueToSet = true;

        //private SerializableAttribute

        void OnTriggerEnter2D(Collider2D other)
        {
            if (onlyOnce && _done)
            {
                return;
            }
            target.SetActive(valueToSet);
            _done = true;
        }
    }
}
