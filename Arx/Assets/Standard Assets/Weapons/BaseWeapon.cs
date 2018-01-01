using CommonInterfaces.Weapons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Standard_Assets.Weapons
{
    public abstract class BaseWeapon : ScriptableObject, IWeapon
    {
        [SerializeField]
        private CommonVisualWeaponComponent _leftHandWeapon;
        [SerializeField]
        private CommonVisualWeaponComponent _rightHandWeapon;

        public WeaponType WeaponType { get; protected set; }
        public CommonVisualWeaponComponent LeftHandWeapon
        {
            get
            {
                return _leftHandWeapon;
            }
        }
        public CommonVisualWeaponComponent RightHandWeapon
        {
            get
            {
                return _rightHandWeapon;
            }
        }
        public GameObject LeftHandSocket { get; set; }
        public GameObject RightHandSocket { get; set; }

        public virtual void Equipped() { }
        public virtual void Unequipped() { }
    }
}
