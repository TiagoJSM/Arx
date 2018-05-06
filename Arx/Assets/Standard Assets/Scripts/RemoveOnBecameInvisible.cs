using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Standard_Assets.Scripts
{
    public class RemoveOnBecameInvisible : MonoBehaviour
    {
        [SerializeField]
        private GameObject _root;

        private void OnBecameInvisible()
        {
            Destroy(_root);
        }
    }
}
