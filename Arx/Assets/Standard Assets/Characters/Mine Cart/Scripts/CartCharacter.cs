using Assets.Standard_Assets.Characters.CharacterBehaviour;
using Assets.Standard_Assets.Effects.DamageFlash.Scripts;
using Assets.Standard_Assets.Scripts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Collections;
using Assets.Standard_Assets._2D.Cameras.Scripts;

namespace Assets.Standard_Assets.Characters.Mine_Cart.Scripts
{
    [RequireComponent(typeof(DefaultColorFlash))]
    [RequireComponent(typeof(CharacterStatus))]
    public class CartCharacter : MonoBehaviour, ICharacter
    {
        private CameraShake _cameraShake;
        private CharacterStatus _status;
        private DefaultColorFlash _flash;
        private Vector3 _cameraDefaultPosition;

        [SerializeField]
        private float _shakeAmount = 0.5f;
        [SerializeField]
        private float _shakeDuration = 0.3f;

        public bool CanBeAttacked { get; private set; }
        public GameObject CharacterGameObject { get { return gameObject; } }
        public bool InPain { get { return false; } }
        public int LifePoints { get { return _status.health.lifePoints; } }
        public int MaxLifePoints { get { return _status.health.maxLifePoints; } }

        public int Attacked(GameObject attacker, int damage, Vector3? hitPoint, DamageType damageType, AttackTypeDetail attackType = AttackTypeDetail.Generic, int comboNumber = 1, bool showDamaged = false)
        {
            if (CanBeAttacked)
            {
                _status.Damage(damage);
                _cameraShake.ShakeCamera(_shakeAmount, _shakeDuration, OnCameraStopShaking);
                StartCoroutine(DamagedRoutine());
                return damage;
            }
            return 0;
        }

        public void EndGrappled()
        {
        }

        public void Kill()
        {
        }

        public bool StartGrappled(GameObject grapple)
        {
            return false;
        }

        private void Awake()
        {
            CanBeAttacked = true;
            _status = GetComponent<CharacterStatus>();
            _flash = GetComponent<DefaultColorFlash>();
            _cameraShake = Camera.main.GetComponent<CameraShake>();
            _cameraDefaultPosition = Camera.main.transform.position;
        }

        private IEnumerator DamagedRoutine()
        {
            CanBeAttacked = false;
            yield return _flash.FlashRoutine();
            CanBeAttacked = true;
        }

        private void OnCameraStopShaking()
        {
            Camera.main.transform.position = _cameraDefaultPosition;
        }
    }
}
