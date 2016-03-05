using AnimatorSequencer;
using AnimatorSequencerEditors.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace AnimatorSequencerEditors.AnimationSequence
{
    public static class AnimatorSequenceSaveManager
    {
        public static void Save(string path, AnimationSequenceNode root)
        {
            path = path.Replace(Application.dataPath, "Assets");
            var nodes = root.GetAllSequenceNodes().Where(n => n != root).ToList();
            AssetDatabase.CreateAsset(root, path);
            foreach (var node in nodes)
            {
                node.hideFlags = HideFlags.HideInHierarchy;
                node.state.hideFlags = HideFlags.HideInHierarchy;
                AssetDatabase.AddObjectToAsset(node, root);
                AssetDatabase.AddObjectToAsset(node.state, root);
            }
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}
