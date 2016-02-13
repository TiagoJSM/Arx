using GenericComponents.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GenericComponents.Zones
{
    public class DeathZone : BaseZone
    {
        protected override void OnZoneEnter()
        {
            //Must think about this, the character needs to die someway, 
            //or should the spawner simply move the existing one to a new place?
            LevelManager.Instance.RestartCurrentLevelFromCheckpoint();
        }
    }
}
