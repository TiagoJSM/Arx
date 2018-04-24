using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Standard_Assets.Environment.Hazards.Crusher.Scripts
{
    public class SingleCrusher : MonoBehaviour
    {
        [SerializeField]
        private CrusherController _crusher;

        private void Start()
        {
            StartCoroutine(_crusher.Crush());
        }
    }
}
