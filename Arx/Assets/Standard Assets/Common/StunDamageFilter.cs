using CommonInterfaces.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Standard_Assets.Common
{
    [Serializable]
    public struct StunDamageFilter
    {
        public DamageType damageType;
        public AttackTypeDetail attackTypeDetail;
        public int combo;
    }
}
