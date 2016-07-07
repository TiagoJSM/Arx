using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommonInterfaces.Weapons;

namespace ArxGame.Components.Weapons
{
    public class Sword : EnemyDetection
    {
        public Sword()
        {
            WeaponType = WeaponType.Sword;
        }
    }
}
