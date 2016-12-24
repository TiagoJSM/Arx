using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Extensions;

namespace Assets.Standard_Assets._2D.Scripts.Interaction
{
    public class PlayAnimatorTrigger : MonoBehaviour
    {
        private bool _played;

        [SerializeField]
        private Animator _animator;
        [SerializeField]
        private string _animationStateName;
        [SerializeField]
        private bool _onlyOnce;
        [SerializeField]
        private LayerMask platformMask;

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (!platformMask.IsInAnyLayer(col.gameObject))
            {
                return;
            }

            if(_onlyOnce && _played)
            {
                return;
            }

            _animator.Play(_animationStateName);
            _played = true;
        }
    }
}
