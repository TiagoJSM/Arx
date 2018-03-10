using CommonInterfaces.Controllers;
using CommonInterfaces.Enums;
using MathHelper;
using MathHelper.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace GenericComponents.Helpers
{
    public delegate void AttackFinished();

    public interface ICombatComponent : IAnimationController
    {
        void NotifyAttackFinish();
    }
}
