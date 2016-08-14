using CommonInterfaces.Enums;
using CommonInterfaces.Weapons;
using MathHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ArxGame.Components.Combat
{
    public class ChainThrowCombatBehaviour : BaseGenericCombatBehaviour<IChainThrowWeapon>
    {
        private IChainThrowWeapon _weapon;

        public GameObject aimingArm;

        public override IChainThrowWeapon Weapon
        {
            get
            {
                return _weapon;
            }
            set
            {
                if(_weapon == value)
                {
                    return;
                }
                if(_weapon != null)
                {
                    _weapon.OnAttackFinish -= OnAttackFinishHandler;
                }
                _weapon = value;
                if (_weapon != null)
                {
                    _weapon.OnAttackFinish += OnAttackFinishHandler;
                }
            }
        }
        public override event Action OnAttackFinish;

        public override void PrimaryAttack()
        {
            //throw new NotImplementedException();
        }

        public override void SecundaryAttack()
        {
            //throw new NotImplementedException();
        }

        public void ThrowChain()
        {
            var inverted = (aimingArm.transform.lossyScale.x < 0) || (aimingArm.transform.lossyScale.y < 0);
            Weapon.Throw(inverted ? Direction.Left : Direction.Right);
        }

        void Awake()
        {
            this.enabled = false;
        }

        private void OnAttackFinishHandler()
        {
            OnAttackFinish?.Invoke();
        }
    }
}
