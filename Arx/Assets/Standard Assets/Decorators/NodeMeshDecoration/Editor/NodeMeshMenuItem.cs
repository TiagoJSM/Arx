using Decorator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Assets.Standard_Assets.Decorators.NodeMeshDecoration.Editor
{
    public class NodeMeshMenuItem
    {
        private const string DefaultName = "NodeMesh";
        private static readonly Vector2 DefaultFirstVector = new Vector2();
        private static readonly Vector2 DefaultSecondVector = new Vector2(1, 0);

        [MenuItem("GameObject/2D Object/Node mesh")]
        static void Init()
        {
            var gameObj = new GameObject(DefaultName);
            var view = SceneView.currentDrawingSceneView ?? SceneView.lastActiveSceneView;
            var nodeMesh = gameObj.AddComponent<NodeMesh>();
            nodeMesh.mesh = new Mesh();
            gameObj.AddComponent<MeshFilter>();
            gameObj.AddComponent<MeshRenderer>();
            nodeMesh.AddPathNode(DefaultFirstVector);
            nodeMesh.AddPathNode(DefaultSecondVector);
            if (view != null)
            {
                gameObj.transform.position = view.camera.transform.position;
                var position = gameObj.transform.position;
                position.z = 0;
                gameObj.transform.position = position;
            }
        }
    }
}
