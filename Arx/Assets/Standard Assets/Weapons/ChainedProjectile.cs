using CommonInterfaces.Controllers;
using CommonInterfaces.Enums;
using Extensions;
using GenericComponents.Behaviours;
using MathHelper.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Standard_Assets.Weapons
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
        public event Action<Collider2D, ChainedProjectile, Vector3> OnTriggerEnter;

        private Coroutine _coroutine;
        private int _damage;
        private bool _attached;
        private Collider2D _collider;

        public float threshold;
        public float duration = 10;
        public float distance = 20;
        

        public GameObject Origin { get; set; }
        public ProjectileStatus Status { get; private set; }

        private float MovementPerSeconds
        {
            get
            {
                return distance / duration / 2;
            }
        }

        public bool Throw(float degrees, int damage)
        {
            _damage = damage;

            if (_coroutine == null)
            {
                _attached = false;
                this.gameObject.SetActive(true);
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
            _collider.enabled = false;
            this.transform.position = Origin.transform.position;
        }

        public void Return()
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

            _collider.enabled = true;
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
                    _collider.enabled = false;
                    if(OnAttackFinish != null)
                    {
                        OnAttackFinish();
                    }
                    yield break;
                }
                yield return new WaitForEndOfFrame();
            }
        }

        

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.isTrigger)
            {
                return;
            }
            if(OnTriggerEnter != null)
            {
                OnTriggerEnter(other, this, this.transform.position);
            }
        }

        private void Awake()
        {
            _collider = GetComponent<Collider2D>();
        }

        private void LateUpdate()
        {
            if(Status == ProjectileStatus.None)
            {
                this.transform.position = Origin.transform.position;
            }
        }
    }
}
