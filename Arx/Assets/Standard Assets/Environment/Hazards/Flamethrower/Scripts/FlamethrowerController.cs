using Assets.Standard_Assets.Extensions;
using CommonInterfaces.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Extensions;
using GC = GenericComponents.Controllers.Characters;

namespace Assets.Standard_Assets.Environment.Hazards.Flamethrower.Scripts
{
    public class FlamethrowerController : MonoBehaviour
    {
        private float? _intersectionHeight;
        private RaycastHit2D[] _hits = new RaycastHit2D[10];

        [SerializeField]
        private BoxCollider2D _boxCollider;
        [SerializeField]
        private float _height = 5;
        [SerializeField]
        private int _damage = 1;

        private Vector2 HitDetectionStart
        {
            get
            {
                return transform.position;
            }
        }
        private Vector2 HitDetectionEnd
        {
            get
            {
                var start = HitDetectionStart;
                var end = start - new Vector2(0, _intersectionHeight ?? _height);
                return end.RotateAround(start, Mathf.Deg2Rad * transform.rotation.eulerAngles.z);
            }
        }

        private void FixedUpdate()
        {
            CheckIntersections();
            //DealDamage();
        }

        private void CheckIntersections()
        {
            var hits =
                Physics2D
                    .LinecastAll(HitDetectionStart, HitDetectionEnd)
                    .Where(hit => !hit.collider.isTrigger)
                    .ToArray();

            if (hits.Length == 0)
            {
                _intersectionHeight = null;
                return;
            }
            var position = this.transform.position.ToVector2();
            var closestCollider =
                hits
                    .MinBy(hit => Vector2.Distance(position, hit.point));
            _intersectionHeight = Vector2.Distance(position, closestCollider.transform.position);
            if(_intersectionHeight > _height)
            {
                _intersectionHeight = _height;
            }
            _boxCollider.offset = new Vector2(_boxCollider.offset.x, -_intersectionHeight.Value / 2);
            _boxCollider.size = new Vector2(_boxCollider.size.x, _intersectionHeight.Value);
        }

        private void OnTriggerEnter2D(Collider2D collider)
        {
            var character = collider.GetComponent<GC.CharacterController>();
            if(character == null)
            {
                return;
            }
            var hitCount = _boxCollider.Cast(Vector2.zero, _hits);
            for(var idx = 0; idx < hitCount; idx++)
            {
                if(_hits[idx].collider == collider)
                {
                    character.Attacked(gameObject, _damage, _hits[idx].point, DamageType.Environment);
                    return;
                }
            }
        }

        private void DealDamage(RaycastHit2D[] hits)
        {
            if (hits.Length == 0)
            {
                return;
            }
            var characters = hits.GetCharacters();
            for (var idx = 0; idx < characters.Length; idx++)
            {
                var hit = characters[idx].Item1;
                characters[idx].Item2.Attacked(gameObject, _damage, hit.point, DamageType.Environment);
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(HitDetectionStart, HitDetectionEnd);
        }
    }
}
