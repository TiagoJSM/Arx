using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace GenericComponents.Behaviours
{
    public class SetEnabledTrigger : MonoBehaviour
    {
        private bool _done;

        public Behaviour target;
        public bool onlyOnce;
        public bool valueToSet = true;

        void OnTriggerEnter2D(Collider2D other)
        {
            if(onlyOnce && _done)
            {
                return;
            }
            target.enabled = valueToSet;
            _done = true;
        }
    }
}
