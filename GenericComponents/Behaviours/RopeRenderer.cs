using Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace GenericComponents.Behaviours
{
    [RequireComponent(typeof(Rope))]
    [RequireComponent(typeof(LineRenderer))]
    public class RopeRenderer : MonoBehaviour
    {
        private Rope _rope;
        private LineRenderer _renderer;

        void Awake()
        {
            _rope = GetComponent<Rope>();
            _renderer = GetComponent<LineRenderer>();
        }
        void Update()
        {
            _renderer.SetVertexCount(_rope.Points.Count());
            _renderer.SetPositions(_rope.Points.Select(p => p.ToVector3()).ToArray());
        }
    }
}
