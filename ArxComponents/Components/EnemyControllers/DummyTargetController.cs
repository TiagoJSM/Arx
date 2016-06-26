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
        public override bool Attacked(GameObject attacker, int damage, Vector3? hitPoint)
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
