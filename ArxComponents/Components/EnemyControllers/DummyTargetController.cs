using CommonInterfaces.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ArxGame.Components.EnemyControllers
{
    public class DummyTargetController : BaseEnemyController
    {
        public override bool Attack(ICharacter target, Vector3? hitPoint)
        {
            return false;
        }

        public override bool Attacked(ICharacter attacker, int damage, Vector3? hitPoint)
        {
            CharacterStatus.Damage(damage);
            if (CharacterStatus.HealthDepleted)
            {
                Kill();
            }
            return true;
        }
    }
}
