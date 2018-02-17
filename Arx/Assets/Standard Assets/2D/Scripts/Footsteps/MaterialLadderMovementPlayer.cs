using Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Standard_Assets._2D.Scripts.Footsteps
{
    public class MaterialLadderMovementPlayer : MonoBehaviour
    {
        private GroundMaterial? _material;
        public AudioMaterialMap audioMaterial;

        public MaterialLadderMovementPlayer()
        {
            audioMaterial = new AudioMaterialMap();
        }

        public void GrabLadder(GameObject ladder)
        {
            var ground = ladder.GetComponent<GroundComponent>();
            if(ground != null)
            {
                _material = ground.groundMaterial;
            }
        }

        public void PlayMovementSound()
        {
            var audioClips = audioMaterial.GetFor(_material);
            if (audioClips == null || audioClips.Length == 0)
            {
                return;
            }
            var audio = audioClips.Random();
            audio.Play();
        }
    }
}
