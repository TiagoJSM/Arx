using CommonInterfaces.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Standard_Assets._2D.Scripts.Controllers
{
    [RequireComponent(typeof(PlatformerCharacterController))]
    public class WallJumpDetector : MonoBehaviour
    {
        private PlatformerCharacterController _platformerCharacter;

        [SerializeField]
        private LayerMask _wallMask;
        [SerializeField]
        private int _rayCastCount = 3;
        [SerializeField]
        private float _distance = 1;
        [SerializeField]
        private float _heightOffset = 1f;

        public bool WallDetected()
        {
            var characterController = _platformerCharacter.CharacterController2D;
            var modifiedBounds = characterController.BoxCollider2D.bounds;
            var colliderHeight = (characterController.BoxCollider2D.size.y * Mathf.Abs(transform.lossyScale.y)) - (_heightOffset);
            var heightBetweenRays = _rayCastCount < 2 
                ? colliderHeight 
                : colliderHeight / (_rayCastCount - 1);
            modifiedBounds.Expand(-2f * characterController.skinWidth);

            var origin = _platformerCharacter.Direction == Direction.Left
                ? modifiedBounds.min.ToVector2()
                : new Vector2(modifiedBounds.max.x, modifiedBounds.min.y);

            origin += new Vector2(0, _heightOffset / 2);

            var direction = _platformerCharacter.Direction == Direction.Left
                ? Vector2.left
                : Vector2.right;

            for(var idx = 0; idx < _rayCastCount; idx++)
            {
                var start = origin + new Vector2(0, idx * heightBetweenRays);
                var hit  = Physics2D.Raycast(start, direction, _distance, _wallMask);
                Debug.DrawRay(start, direction * _distance, Color.blue);
                if (!hit)
                {
                    return false;
                }
            }
            return true;
        }

        private void Awake()
        {
            _platformerCharacter = GetComponent<PlatformerCharacterController>();
        }
    }
}
