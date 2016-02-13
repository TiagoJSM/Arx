using GenericComponents.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GenericComponents.Zones
{
    public class LevelChangeZone : BaseZone
    {
        public string sceneName;

        protected override void OnZoneEnter()
        {
            if(sceneName == null)
            {
                Debug.LogError("Scene name is not populated");
                return;
            }
            LevelManager.Instance.GoToScene(sceneName);
        }
    }
}
