using Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Standard_Assets._2D.Scripts.Footsteps
{
    public class MaterialRopeMovementPlayer : MonoBehaviour
    {
        private GroundMaterial? _material;
        public AudioMaterialMap audioMaterial;

        public MaterialRopeMovementPlayer()
        {
            audioMaterial = new AudioMaterialMap();
        }

        public void GrabRope(GameObject rope)
        {
            var ground = rope.GetComponent<GroundComponent>();
            if (ground != null)
            {
                _material = ground.groundMaterial;
            }
        }

        public void PlayRopeMovementSound()
        {
            var audioClip = audioMaterial.GetRandomFor(_material);
            if (audioClip != null)
            {
                audioClip.Play();
            }
        }
    }
}
