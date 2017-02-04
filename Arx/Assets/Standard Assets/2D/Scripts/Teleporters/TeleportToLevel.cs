using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Extensions;
using UnityEngine.SceneManagement;
using Assets.Standard_Assets.Common.Attributes;
using Assets.Standard_Assets.Common;
using Assets.Standard_Assets._2D.Scripts.Managers;

namespace Assets.Standard_Assets._2D.Scripts.Teleporters
{
    public class TeleportToLevel : MonoBehaviour
    {
        [SerializeField]
        private LayerMask _detectorMask;
        [SceneField]
        [SerializeField]
        private SceneField _scene;
        [SerializeField]
        private string _location;

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (_detectorMask.IsInAnyLayer(collider.gameObject))
            {
                LevelManager.Instance.GoToScene(_scene.SceneName, _location);
            }
        }
    }
}
