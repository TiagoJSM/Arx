using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Standard_Assets.Common
{
    [System.Serializable]
    public class SceneField
    {
        [SerializeField]
        private UnityEngine.Object m_SceneAsset;

        [SerializeField]
        private string m_SceneName = "";
        public string SceneName
        {
            get { return m_SceneName; }
        }

        // makes it work with the existing Unity methods (LoadLevel/LoadScene)
        public static implicit operator string(SceneField sceneField)
        {
            return sceneField.SceneName;
        }
    }
}
