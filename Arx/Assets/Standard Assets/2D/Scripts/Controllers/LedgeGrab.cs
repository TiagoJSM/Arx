using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Standard_Assets._2D.Scripts.Controllers
{
    [RequireComponent(typeof(PlatformerCharacterController))]
    [RequireComponent(typeof(LedgeChecker))]
    public class LedgeGrab : MonoBehaviour
    {
        private LedgeChecker _ledgeChecker;
        private PlatformerCharacterController _platformerCharacter;

        private Collider2D _detectedLedge;
        private Collider2D _lastGrabbedLedge;
        private bool _ledgeDetected;
        private float _defaultGravity;

        [SerializeField]
        private AudioSource _grabLedgeSound;

        public bool GrabbingLedge { get; private set; }

        private bool DetectingPreviousGrabbedLedge
        {
            get
            {
                if (_lastGrabbedLedge == null)
                {
                    return false;
                }
                return _lastGrabbedLedge == _detectedLedge;
            }
        }

        public void LedgeDetected(bool detected, Collider2D ledgeCollider)
        {
            _ledgeDetected = detected;
            if (!detected)
            {
                if (GrabbingLedge)
                {
                    DropLedge();
                }
                _lastGrabbedLedge = null;
                return;
            }
            _detectedLedge = ledgeCollider;
        }

        public bool CanGrabLedge
        {
            get
            {
                return _ledgeDetected && !_platformerCharacter.IsGrounded && !DetectingPreviousGrabbedLedge && !GrabbingLedge;
            }
        }

        public void DoUpdate()
        {
            Collider2D collider;
            Vector3 pos;
            var ledgeDetected = _ledgeChecker.IsLedgeDetected(out collider, out pos);
            LedgeDetected(ledgeDetected, collider);
        }

        public void DoGrabLedge()
        {
            if (_detectedLedge == null)
            {
                return;
            }

            _lastGrabbedLedge = _detectedLedge;
            _platformerCharacter.SupportingPlatform = _lastGrabbedLedge.gameObject.transform;
            _platformerCharacter.DesiredMovementVelocity = Vector2.zero;
            _platformerCharacter.ApplyMovementAndGravity = false;
            _platformerCharacter.CharacterController2D.velocity = Vector2.zero;
            //_platformerCharacter.gravity = 0;
            //gravity = 0;
            GrabbingLedge = true;
            _grabLedgeSound.Play();
        }

        public void DropLedge()
        {
            GrabbingLedge = false;
            _platformerCharacter.SupportingPlatform = null;
            _platformerCharacter.ApplyMovementAndGravity = true;
            //_platformerCharacter.gravity = _defaultGravity;
            //gravity = _defaultGravity;
        }

        private void Awake()
        {
            _platformerCharacter = GetComponent<PlatformerCharacterController>();
            _ledgeChecker = GetComponent<LedgeChecker>();
            _defaultGravity = _platformerCharacter.gravity;
        }

        //private void LateUpdate()
        //{
        //    Collider2D collider;
        //    Vector3 pos;
        //    var ledgeDetected = _ledgeChecker.IsLedgeDetected(out collider, out pos);
        //    LedgeDetected(ledgeDetected, collider);
        //}
    }
}
