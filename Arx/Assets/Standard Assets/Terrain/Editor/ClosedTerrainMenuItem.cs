using Assets.Standard_Assets.Extensions;
using Assets.Standard_Assets.Terrain.Editor.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Assets.Standard_Assets.Terrain.Editor
{
    public class ClosedTerrainMenuItem
    {
        [MenuItem("GameObject/2D Object/Terrain/Closed Terrain")]
        static void Init()
        {
            TerrainEditorUtils.InitializeTerrain<ClosedTerrainField>(false);
        }
    }
}
