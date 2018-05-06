using Assets.Standard_Assets.Weapons;
using MathHelper.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Standard_Assets.Characters.Mine_Cart.Scripts
{
    public class CartCannon : MonoBehaviour
    {
        private float _currentDelay;

        [SerializeField]
        private GameObject _cannon;
        [SerializeField]
        private Projectile _projectile;
        [SerializeField]
        private float _canonDelay = 0.35f;

        private void Update()
        {
            var inputDevice = InputManager.Instance.GetInputDevice();
            var rotation = GetRotation(inputDevice);
            _cannon.transform.rotation = Quaternion.Euler(0, 0, rotation);

            if(_currentDelay > 0.0)
            {
                _currentDelay -= Time.deltaTime;
            }

            if (inputDevice.GetButtonDown(DeviceButton.PrimaryAttack) && _currentDelay <= 0)
            {
                var projectile = Instantiate(_projectile, _cannon.transform.position, Quaternion.identity);
                var direction = rotation.GetDirectionVectorFromDegreeAngle();
                projectile.Direction = direction;
                _currentDelay = _canonDelay;
            }
        }

        private float GetRotation(IInputDevice inputDevice)
        {
            if (inputDevice.MouseSupport)
            {
                var center = _cannon.transform.position;
                var aimPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                var pos = aimPosition - center;
                var degrees = (Vector2.Angle(Vector2.right, pos) * Math.Sign(pos.y)).ReduceToSingleTurn();
                return degrees;
            }
            else
            {
                var x = inputDevice.GetAxis(DeviceAxis.AimAnalog).x;
                var y = inputDevice.GetAxis(DeviceAxis.AimAnalog).y;
                var angle = Vector2.Angle(Vector2.right, new Vector2(x, y));
                if (y < 0)
                {
                    angle = 360 - angle;
                }
                return angle;
            }
        }
    }
}
