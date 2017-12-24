using Assets.Standard_Assets.Common.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Standard_Assets.Common
{
    [ExecuteInEditMode]
    public class UniqueId : MonoBehaviour
    {
        [SerializeField]
        [ReadOnly]
        private string _id;

        public string Id
        {
            get { return _id; }
        }

#if UNITY_EDITOR
        private void Awake()
        {
            if(_id == null)
            {
                _id = GetUniqueId();
                return;
            }

            var sceneNameFromId = GetSceneFromIdName();

            if(gameObject.scene.name != sceneNameFromId)
            {
                _id = GetUniqueId();
                return;
            }

            if (IsIdInUse(_id))
            {
                _id = GetUniqueId();
            }
        }

        private string GetUniqueId()
        {
            var ids = FindObjectsOfType<UniqueId>();

            while (true)
            {
                var generatedId = gameObject.scene.name + "_" + Guid.NewGuid();

                if (!IsIdInUse(generatedId))
                {
                    return generatedId;
                }
            }
        }

        private bool IsIdInUse(string id)
        {
            var ids = FindObjectsOfType<UniqueId>();

            for (var idx = 0; idx < ids.Length; idx++)
            {
                var uniqueId = ids[idx];

                if (uniqueId == this)
                {
                    continue;
                }

                if (uniqueId.Id == id)
                {
                    return true;
                }
            }
            return false;
        }

        private string GetSceneFromIdName()
        {
            int idx = _id.LastIndexOf('_');
            return _id.Substring(0, idx);
        }
    }
#endif
}
