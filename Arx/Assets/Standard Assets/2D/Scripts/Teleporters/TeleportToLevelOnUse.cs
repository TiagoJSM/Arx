using Assets.Standard_Assets._2D.Scripts.Managers;
using Assets.Standard_Assets.Common;
using Assets.Standard_Assets.Common.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Assets.Standard_Assets.Environment.Teleports.Teleport_On_Use.Scripts;

namespace Assets.Standard_Assets._2D.Scripts.Teleporters
{
    public class TeleportToLevelOnUse : MonoBehaviour, ITeleporter
    {
        [SceneField]
        [SerializeField]
        private SceneField _scene;
        [SerializeField]
        private string _location;
        [SerializeField]
        private TeleportOnUseNotification _notification;

        public TeleportOnUseNotification Notification
        {
            get
            {
                return _notification; ;
            }
        }

        public void Teleport(GameObject teleportTarget)
        {
            LevelManager.Instance.GoToScene(_scene.SceneName, _location);
        }
    }
}
