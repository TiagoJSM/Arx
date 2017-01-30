using MathHelper.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Standard_Assets._2D.Platforms.Hinge_Platform.Scripts
{
    public class HingePlatformController : MonoBehaviour
    {
        private Coroutine _returnToRestRoutine;
        private Coroutine _balancePlatformCoroutine;
        private Collision2D _collision;

        [SerializeField]
        private float _rotationToRestPerSecond = 30;
        [SerializeField]
        private float _minBalanceRotation = 1;
        [SerializeField]
        private float _balanceRotationPerUnitPerSecond = 1;
        [SerializeField]
        private float _restAngleThreshold = 1;

        private void Awake()
        {
            //_body = GetComponent<Rigidbody2D>();
            //_joint = GetComponent<HingeJoint2D>();
        }

        private void OnCollisionEnter2D(Collision2D coll)
        {
            _collision = coll;
            StopRestRotation();
        }

        private void OnCollisionStay2D(Collision2D coll)
        {
            _collision = coll;
        }

        private void OnCollisionExit2D(Collision2D coll)
        {
            _collision = null;
            RotateToRest();
        }

        private void RotateToRest()
        {
            if (_balancePlatformCoroutine != null)
            {
                StopCoroutine(_balancePlatformCoroutine);
                _balancePlatformCoroutine = null;
            }
            if (_returnToRestRoutine == null)
            {
                _returnToRestRoutine = StartCoroutine(ReturnToRest());
            }
        }

        private void StopRestRotation()
        {
            if (_balancePlatformCoroutine == null)
            {
                _balancePlatformCoroutine = StartCoroutine(BalancePlatform());
            }
            if (_returnToRestRoutine != null)
            {
                StopCoroutine(_returnToRestRoutine);
                _returnToRestRoutine = null;
            }
        }

        private bool IsInRestRotation()
        {
            float rotation = transform.rotation.eulerAngles.z;
            if (Math.Abs(rotation) < _restAngleThreshold)
            {
                return true;
            }
            if (Math.Abs(rotation - 360) < _restAngleThreshold)
            {
                return true;
            }
            return false;
        }

        private IEnumerator ReturnToRest()
        {
            while (!IsInRestRotation())
            {
                float rotation = transform.rotation.eulerAngles.z;
                var rotationDirection = rotation > 180 ? 1 : -1;
                Rotate(_rotationToRestPerSecond * Time.deltaTime * rotationDirection);
                yield return null;
            }
            SetRotation(0);
            _returnToRestRoutine = null;
        }

        private IEnumerator BalancePlatform()
        {
            while (true)
            {
                var centerXPoint = _collision.contacts.Sum(c => c.point.x) / _collision.contacts.Length;
                var centerYPoint = _collision.contacts.Sum(c => c.point.y) / _collision.contacts.Length;
                var distanceToContact = Mathf.Abs(centerXPoint - transform.position.x);
                var rotationDirection = centerXPoint < transform.position.x ? 1 : -1;
                //rotationDirection *= (int)-Mathf.Sign(_collision.contacts[0].normal.y);
                var balanceRotation = Mathf.Max(_balanceRotationPerUnitPerSecond * distanceToContact, _minBalanceRotation);
                Rotate(balanceRotation * Time.deltaTime * rotationDirection);
                yield return null;
            }
        }

        private void Rotate(float z)
        {
            var euler = transform.rotation.eulerAngles;
            euler.z += z;
            euler.z = euler.z.ReduceToSingleTurn();
            var quaternion = transform.rotation;
            quaternion.eulerAngles = euler;
            transform.rotation = quaternion;
        }

        private void SetRotation(float z)
        {
            var euler = transform.rotation.eulerAngles;
            euler.z = z;
            euler.z = euler.z.ReduceToSingleTurn();
            var quaternion = transform.rotation;
            quaternion.eulerAngles = euler;
            transform.rotation = quaternion;
        }
    }
}
