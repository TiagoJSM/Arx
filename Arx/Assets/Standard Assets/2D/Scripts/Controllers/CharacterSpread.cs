using Assets.Standard_Assets._2D.Scripts.Characters.Enemies;
using Assets.Standard_Assets.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Standard_Assets._2D.Scripts.Controllers
{
    [RequireComponent(typeof(PlatformerCharacterController))]
    public class CharacterSpread : MonoBehaviour
    {
        private PlatformerCharacterController _character;
        private LayerMask _characterMasks;

        [SerializeField]
        private float _spreadMovement = 20.0f;

        private void Awake()
        {
            _character = GetComponent<PlatformerCharacterController>();
            _characterMasks = LayerMask.GetMask(GameObjectExtensions.EnemyTag, GameObjectExtensions.PlayerTag);
        }

        private void Update()
        {
            SpreadWhenFalling();
            MoveWhenOnTopOfEnemies();
        }

        private void MoveWhenOnTopOfEnemies()
        {
            if (_character.Velocity.y > 0)
            {
                return;
            }
            if (_character.SupportingPlatform && _character.IsGrounded)
            {
                return;
            }

            var myBounds = _character.CharacterController2D.BoxCollider2D.bounds;
            var colliders = Physics2D.OverlapBoxAll(transform.position, new Vector2(myBounds.size.x * 2, 1f), 0, _characterMasks);
            var myCenter = myBounds.center;
            Collider2D closestCollider = null;
            var distance = float.MaxValue;
            for (var idx = 0; idx < colliders.Length; idx++)
            {
                var collider = colliders[idx];
                if (collider.isTrigger)
                {
                    continue;
                }
                if (collider.gameObject == this.gameObject)
                {
                    continue;
                }
                
                var otherCenter = collider.bounds.center;

                var currentDistance = Mathf.Abs(otherCenter.x - myCenter.x);
                if(currentDistance < distance)
                {
                    currentDistance = distance;
                    closestCollider = collider;
                }
            }

            if (closestCollider != null)
            {
                var spreadDirection = Mathf.Sign(myCenter.x - closestCollider.bounds.center.x);
                _character.CharacterController2D.move(new Vector3(spreadDirection * _spreadMovement * Time.deltaTime, 0));
            }
        }

        private void SpreadWhenFalling()
        {
            if (_character.Velocity.y > 0 || _character.SupportingPlatform)
            {
                return;
            }

            var myBounds = _character.CharacterController2D.BoxCollider2D.bounds;
            var colliders = Physics2D.OverlapBoxAll(transform.position, new Vector2(myBounds.size.x * 2, 1), 0, _characterMasks);
            
            for(var idx = 0; idx < colliders.Length; idx++)
            {
                var collider = colliders[idx];
                if (collider.isTrigger)
                {
                    continue;
                }
                if (collider.gameObject == this.gameObject)
                {
                    continue;
                }
                var otherCharacter = collider.GetComponent<CharacterController2D>();
                if (otherCharacter != null && otherCharacter.isGrounded)
                {
                    var myCenter = myBounds.center;
                    var otherCenter = collider.bounds.center;

                    if (otherCharacter != null && myCenter.y > otherCenter.y)
                    {
                        var spreadDirection = Mathf.Sign(otherCenter.x - myCenter.x);
                        otherCharacter.move(new Vector3(spreadDirection * _spreadMovement * Time.deltaTime, 0));
                    }
                }
            }
            
        }
    }
}
