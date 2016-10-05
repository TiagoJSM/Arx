using CommonInterfaces.Enums;
using Extensions;
using MathHelper.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ArxGame.Components.Weapons
{
    public enum ProjectileStatus
    {
        None,
        Throw,
        Return,
        Stuck
    }
    public class ChainedProjectile : MonoBehaviour
    {
        public event Action OnAttackFinish;
        public event Action<Collider2D, ChainedProjectile> OnTriggerEnter;

        private Coroutine _coroutine;

        public float threshold;
        public float duration = 10;
        public float distance = 20;
        public Collider2D collider;

        public GameObject Origin { get; set; }
        public ProjectileStatus Status { get; private set; }

        private float MovementPerSeconds
        {
            get
            {
                return distance / duration / 2;
            }
        }

        public bool Throw(float degrees)
        {
            if(_coroutine == null)
            {
                _coroutine = StartCoroutine(ThrowCoroutine(degrees));
                return true;
            }
            return false;
        }

        public void Stop()
        {
            if (_coroutine != null)
            {
                Status = ProjectileStatus.Stuck;
                StopCoroutine(_coroutine);
            }
        }

        public void Reset()
        {
            if (_coroutine != null)
            {
                StopCoroutine(_coroutine);
                _coroutine = null;
            }
            Status = ProjectileStatus.None;
            collider.enabled = false;
            this.transform.position = Origin.transform.position;
        }

        private void Return()
        {
            if(_coroutine != null)
            {
                StopCoroutine(_coroutine);
            }
            _coroutine = StartCoroutine(ReturnCoroutine());
        }

        private IEnumerator ThrowCoroutine(float degrees)
        {
            var throwDuration = duration / 2;
            var elapsedTime = 0f;
            var direction = degrees.GetDirectionVectorFromDegreeAngle();

            collider.enabled = true;
            Status = ProjectileStatus.Throw;

            while (true)
            {
                elapsedTime += Time.deltaTime;
                if (elapsedTime > throwDuration)
                {
                    Return();
                    yield break;
                }
                this.transform.position = this.transform.position + (direction * MovementPerSeconds * Time.deltaTime);
                yield return null;
            }
        }

        private IEnumerator ReturnCoroutine()
        {
            Status = ProjectileStatus.Return;

            while (true)
            {
                var direction = (Origin.transform.position - this.transform.localPosition).normalized;
                this.transform.position = this.transform.position + direction * MovementPerSeconds * Time.deltaTime;
                if (Vector3.Distance(this.transform.localPosition, Origin.transform.position) < threshold)
                {
                    _coroutine = null;
                    Status = ProjectileStatus.None;
                    collider.enabled = false;
                    OnAttackFinish?.Invoke();
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
            OnTriggerEnter?.Invoke(other, this);
            Return();
        }

        void LateUpdate()
        {
            if(Status == ProjectileStatus.None)
            {
                this.transform.position = Origin.transform.position;
            }
        }
    }
}
