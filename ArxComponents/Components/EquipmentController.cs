using ArxGame.Components.Weapons;
using CommonInterfaces.Weapons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ArxGame.Components
{
    public class EquipmentController : MonoBehaviour
    {
        private GameObject _equippedWeapon;

        public GameObject startingWeapon;
        public GameObject weaponSocket;

        public IWeapon EquippedWeapon
        {
            get
            {
                if(_equippedWeapon == null)
                {
                    return null;
                }
                return _equippedWeapon.GetComponent<IWeapon>();
            }
        }

        void Awake()
        {
            if(startingWeapon != null)
            {
                EquipWeapon(startingWeapon);
            }
        }

        public void EquipWeapon(GameObject weaponObject)
        {
            var weapon = weaponObject.GetComponent<IWeapon>();
            if(weapon == null)
            {
                Debug.LogError("GameObject is not a weapon");
                return;
            }
            if (_equippedWeapon != null)
            {
                Destroy(_equippedWeapon);
            }
            _equippedWeapon = Instantiate(weaponObject);
            //_equippedWeapon.Owner = this.gameObject;
            _equippedWeapon.transform.SetParent(weaponSocket.transform, false);
        }
    }
}
