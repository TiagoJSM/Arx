using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ArxGame.Components.Weapons
{
    public class ChainedProjectile : MonoBehaviour
    {
        //duration of throw + return
        

        private Coroutine _current;

        public float threshold;
        public float duration = 10;
        public float distance = 20;
        public GameObject origin;

        private float MovementPerSeconds
        {
            get
            {
                return distance / duration / 2;
            }
        }

        public bool Throw()
        {
            if(_current == null)
            {
                _current = StartCoroutine(ThrowCoroutine());
                return true;
            }
            return false;
        }

        private void Return()
        {
            StopCoroutine(_current);
            _current = StartCoroutine(ReturnCoroutine());
        }

        private IEnumerator ThrowCoroutine()
        {
            var throwDuration = duration / 2;
            var elapsedTime = 0f;
            while (true)
            {
                elapsedTime += Time.deltaTime;
                if (elapsedTime > throwDuration)
                {
                    Return();
                    yield break;
                }
                this.transform.position = this.transform.position + new Vector3(MovementPerSeconds * Time.deltaTime, 0);
                yield return null;
            }
        }

        private IEnumerator ReturnCoroutine()
        {
            while (true)
            {
                var direction = (origin.transform.position - this.transform.localPosition).normalized;
                this.transform.position = this.transform.position + direction * MovementPerSeconds * Time.deltaTime;
                if (Vector3.Distance(this.transform.localPosition, origin.transform.position) < threshold)
                {
                    _current = null;
                    yield break;
                }
                yield return null;
            }
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.isTrigger)
            {
                return;
            }
            Return();
        }
    }
}
