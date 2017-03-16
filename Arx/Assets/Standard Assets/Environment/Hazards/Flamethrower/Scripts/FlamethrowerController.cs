using Assets.Standard_Assets.Extensions;
using CommonInterfaces.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Extensions;

namespace Assets.Standard_Assets.Environment.Hazards.Flamethrower.Scripts
{
    public class FlamethrowerController : MonoBehaviour
    {
        private float? _intersectionHeight;

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

        private void Update()
        {
            var hits =
                Physics2D
                    .LinecastAll(HitDetectionStart, HitDetectionEnd)
                    .Where(hit => !hit.collider.isTrigger)
                    .ToArray();

            CheckIntersections(hits);
            DealDamage(hits);
        }

        private void CheckIntersections(RaycastHit2D[] hits)
        {
            if(hits.Length == 0)
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
