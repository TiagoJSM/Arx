using ArxGame.Components.Weapons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ArxGame.Components
{
    public class EquipmentController : MonoBehaviour
    {
        private EnemyDetection _equippedWeapon;

        public EnemyDetection startingWeapon;
        public GameObject weaponSocket;

        public EnemyDetection EquippedWeapon
        {
            get
            {
                return _equippedWeapon;
            }
        }

        void Awake()
        {
            if(startingWeapon != null)
            {
                EquipWeapon(startingWeapon);
            }
        }

        public void EquipWeapon(EnemyDetection startingWeapon)
        {
            if(_equippedWeapon != null)
            {
                Destroy(_equippedWeapon);
            }
            _equippedWeapon = Instantiate(startingWeapon);
            _equippedWeapon.Owner = this.gameObject;
            _equippedWeapon.transform.SetParent(weaponSocket.transform, false);
        }
    }
}
