using CommonInterfaces.Weapons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ArxGame.Components.Weapons
{
    public abstract class BaseWeapon : ScriptableObject, IWeapon
    {
        [SerializeField]
        private GameObject _leftHandWeapon;
        [SerializeField]
        private GameObject _rightHandWeapon;

        public WeaponType WeaponType { get; protected set; }
        public GameObject LeftHandWeapon
        {
            get
            {
                return _leftHandWeapon;
            }
        }
        public GameObject RightHandWeapon
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
