using Assets.Standard_Assets.Common;
using Assets.Standard_Assets.Common.Attributes;
using GenericComponents.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Standard_Assets._2D.Scripts.Teleporters
{
    public class TeleportToLevelOnUse : MonoBehaviour, ITeleporter
    {
        [SceneField]
        [SerializeField]
        private SceneField _scene;
        [SerializeField]
        private string _location;

        public void Teleport(GameObject teleportTarget)
        {
            LevelManager.Instance.GoToScene(_scene.SceneName, _location);
        }
    }
}
