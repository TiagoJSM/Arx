using Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace GenericComponents.Behaviours
{
    [Serializable]
    public class AudioMaterialMap
    {
        public AudioClip[] defaultFootsteps;
        public AudioClip[] rockFootSteps;
        public AudioClip[] woodFootSteps;

        public AudioClip[] GetFor(GroundMaterial? material)
        {
            if(material == null)
            {
                return defaultFootsteps;
            }

            switch (material.Value)
            {
                case GroundMaterial.Rock:
                    return rockFootSteps;
                case GroundMaterial.Wood:
                    return woodFootSteps;
                default:
                    return defaultFootsteps;
            }
        }
    }

    public class FootstepPlayer : MonoBehaviour
    {
        private GroundMaterial? _material;

        public Transform groundCheck;
        public LayerMask whatIsGround;
        public float groundCheckRadius = 0.2f;

        public AudioMaterialMap audioMaterial;

        public FootstepPlayer()
        {
            audioMaterial = new AudioMaterialMap();
        }

        public void PlayFootsteps()
        {
            var audioClips = audioMaterial.GetFor(_material);
            if(audioClips == null || audioClips.Length == 0)
            {
                return;
            }
            AudioSource.PlayClipAtPoint(audioClips.Random(), groundCheck.position);
        }

        void Update()
        {
            var groundComponent =
                Physics2D
                    .OverlapCircleAll(groundCheck.position, groundCheckRadius, whatIsGround)
                    .Where(c => !c.isTrigger)
                    .Select(c => c.GetComponent<GroundComponent>())
                    .FirstOrDefault(c => c != null);

            _material = groundComponent == null ? default(GroundMaterial?) : groundComponent.groundMaterial;
        }
    }
}
