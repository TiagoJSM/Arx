using Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace GenericComponents.Behaviours
{
    [RequireComponent(typeof(LineRenderer))]
    public class RopeRenderer : MonoBehaviour
    {
        [SerializeField]
        private BaseRope _rope;
        private LineRenderer _renderer;

        public BaseRope Rope
        {
            get
            {
                return _rope;
            }
            set
            {
                _rope = value;
            }
        }

        void Awake()
        {
            _rope = GetComponent<Rope>();
            _renderer = GetComponent<LineRenderer>();
        }
        void Update()
        {
            if(_rope == null)
            {
                _renderer.SetVertexCount(0);
            }
            _renderer.SetVertexCount(_rope.Points.Count());
            _renderer.SetPositions(_rope.Points.Select(p => p.ToVector3()).ToArray());
        }
    }
}
