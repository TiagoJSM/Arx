using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Standard_Assets.UI.HUD.Scripts
{
    [RequireComponent(typeof(Animator))]
    public class HalfHeartController : MonoBehaviour
    {
        [SerializeField]
        private Animator _animator;

        public void Lose()
        {
            _animator.SetTrigger("Lose");
        }

        public void Gain()
        {
            _animator.SetTrigger("Gain");
        }
    }
}
