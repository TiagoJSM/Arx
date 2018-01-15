using Assets.Standard_Assets._2D.Cameras.Scripts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Standard_Assets.Environment.Factory.Scripts
{
    public class ScrapBall : MonoBehaviour
    {
        [SerializeField]
        private CameraShake _camShake;

        public void FallImpact()
        {
            _camShake.ShakeCamera(5.0f, 0.5f);
        }

        public void MoveImpact()
        {
            _camShake.ShakeCamera(2.0f, 0.5f);
        }
    }
}
