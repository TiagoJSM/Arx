using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Standard_Assets.Weapons
{
    public interface IGrapple : IChainThrowWeapon
    {
        void Detatch();
    }
}
