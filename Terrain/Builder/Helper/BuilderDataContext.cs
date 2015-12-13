using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Terrain.Builder.Helper
{
    public class BuilderDataContext
    {
        public List<Vector2> Vertices { get; set; }
        public List<int> Indices { get; set; }
        public List<Vector2> Uvs { get; set; }
        public List<Color> Colors { get; set; }
        public int CurrentIndice
        {
            get
            {
                return Indices.Max();
            }
        }

        public BuilderDataContext()
        {
            Vertices = new List<Vector2>();
            Indices = new List<int>();
            Uvs = new List<Vector2>();
            Colors = new List<Color>();
        }
    }
}
