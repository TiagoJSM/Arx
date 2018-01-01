using Assets.Standard_Assets._2D.Scripts.Characters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Standard_Assets.Characters.CharacterBehaviour
{
    public class CharacterAwarenessNotifier : MonoBehaviour
    {
        private Collider2D[] _colliders = new Collider2D[10];

        [SerializeField]
        private float _radius = 20;
        [SerializeField]
        private LayerMask _layers;

        public void Notify(GameObject obj)
        {
            var count = Physics2D.OverlapCircleNonAlloc(transform.position, _radius, _colliders, _layers);
            for(var idx = 0; idx < count; idx++)
            {
                if(_colliders[idx].gameObject == gameObject)
                {
                    continue;
                }
                var aware = _colliders[idx].GetComponent<ICharacterAware>();
                if(aware != null)
                {
                    aware.Aware(obj);
                }
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, _radius);
        }
    }
}
