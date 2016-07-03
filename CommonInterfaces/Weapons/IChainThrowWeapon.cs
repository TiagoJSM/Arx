﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonInterfaces.Weapons
{
    public interface IChainThrowWeapon : IWeapon
    {
        void Spin();
        void FocusThrow();
        void Throw();
    }
}
