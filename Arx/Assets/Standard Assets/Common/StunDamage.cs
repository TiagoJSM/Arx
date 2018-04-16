using Assets.Standard_Assets.Scripts;
using CommonInterfaces.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Standard_Assets.Common
{
    [Serializable]
    public class StunDamage
    {
        public bool allAttacksStun = true;
        public StunDamageFilter[] stunFilters;

        public bool DoesStun(DamageType damageType, AttackTypeDetail attackTypeDetail, int combo)
        {
            if (allAttacksStun)
            {
                return true;
            }

            for(var idx = 0; idx < stunFilters.Length; idx++)
            {
                var filter = stunFilters[idx];
                if (filter.damageType == damageType && filter.attackTypeDetail == attackTypeDetail && filter.combo == combo)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
