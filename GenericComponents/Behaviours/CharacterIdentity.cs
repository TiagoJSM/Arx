using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace GenericComponents.Behaviours
{
    public class CharacterIdentity : MonoBehaviour
    {
        [SerializeField]
        private string _name;

        public string Name
        {
            get
            {
                return _name;
            }
        }
    }
}
