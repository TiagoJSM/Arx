using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Standard_Assets.Environment.Hazards.Trap_Tile.Scripts
{
    [RequireComponent(typeof(Animator))]
    public class TrapTile : MonoBehaviour
    {
        private Animator _animator;
        private bool _used;

        [SerializeField]
        private AudioSource _activationSound;
        [SerializeField]
        private Animator _hazardAnimator;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!_used)
            {
                _used = true;
                _animator.enabled = true;
                _hazardAnimator.enabled = true;
                _activationSound.Play();
            }
        }
    }
}
