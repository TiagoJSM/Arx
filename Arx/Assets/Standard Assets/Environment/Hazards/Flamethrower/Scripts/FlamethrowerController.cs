using Assets.Standard_Assets.Extensions;
using CommonInterfaces.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Extensions;
using GC = Assets.Standard_Assets._2D.Scripts.Controllers;

namespace Assets.Standard_Assets.Environment.Hazards.Flamethrower.Scripts
{
    public class FlamethrowerController : MonoBehaviour
    {
        private RaycastHit2D[] _hits = new RaycastHit2D[10];

        [SerializeField]
        private float _height = 5;
        [SerializeField]
        private int _damage = 1;
        [SerializeField]
        private LayerMask _hitMask;

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
                var end = start - new Vector2(0, _height);
                return end.RotateAround(start, Mathf.Deg2Rad * transform.rotation.eulerAngles.z);
            }
        }

        private void FixedUpdate()
        {
            var hit = GetClosestHit(HitDetectionStart, HitDetectionEnd);
            if (hit)
            {
                var character = hit.transform.GetComponent<GC.BasePlatformerController>();
                if (character != null)
                {
                    character.Attacked(gameObject, _damage, hit.point, DamageType.Environment);
                }
            }
        }

        private RaycastHit2D GetClosestHit(Vector3 origin, Vector3 target)
        {
            var hitCount = Physics2D.LinecastNonAlloc(origin, target, _hits, _hitMask);
            var hit = default(RaycastHit2D);
            for (var idx = 0; idx < hitCount; idx++)
            {
                if (_hits[idx].collider.isTrigger)
                {
                    continue;
                }
                if (hit)
                {
                    hit =
                        Vector2.Distance(hit.point, origin) < Vector2.Distance(_hits[idx].point, origin)
                            ? hit
                            : _hits[idx];
                }
                else
                {
                    hit = _hits[idx];
                }
            }
            return hit;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(HitDetectionStart, HitDetectionEnd);
        }
    }
}
