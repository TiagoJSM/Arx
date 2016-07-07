using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommonInterfaces.Weapons;

namespace ArxGame.Components.Weapons
{
    public class Fists : EnemyDetection
    {
        public Fists()
        {
            WeaponType = WeaponType.Fist;
        }
    }
}
