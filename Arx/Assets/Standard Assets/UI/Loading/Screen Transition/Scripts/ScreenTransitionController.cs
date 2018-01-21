using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Standard_Assets.UI.Loading.Screen_Transition.Scripts
{
    [RequireComponent(typeof(Animator))]
    public class ScreenTransitionController : MonoBehaviour
    {
        private readonly int FadeInParam = Animator.StringToHash("Fade In");

        private Animator _animator;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        public void FadeIn()
        {
            _animator.SetTrigger(FadeInParam);
        }
    }
}
