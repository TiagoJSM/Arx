﻿using Decorator;
using Decorator.NodeMeshDecoration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace DecoratorEditors.NodeMeshDecoration
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
            var nodeMesh = gameObj.AddComponent<NodeMesh>();
            nodeMesh.mesh = new Mesh();
            gameObj.AddComponent<MeshFilter>();
            gameObj.AddComponent<MeshRenderer>();
            nodeMesh.AddPathNode(DefaultFirstVector);
            nodeMesh.AddPathNode(DefaultSecondVector);
        }
    }
}
