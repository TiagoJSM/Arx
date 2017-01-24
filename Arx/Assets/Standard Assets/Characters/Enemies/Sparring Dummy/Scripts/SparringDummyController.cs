using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Standard_Assets.Characters.Enemies.Sparring_Dummy.Scripts
{
    public class SparringDummyController : PlatformerCharacterController
    {
        protected override void Update()
        {
            base.Update();
            StayStill();
        }
    }
}
