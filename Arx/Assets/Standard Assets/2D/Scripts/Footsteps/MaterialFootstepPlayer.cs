using Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Standard_Assets._2D.Scripts.Footsteps
{
    [Serializable]
    public class AudioMaterialMap
    {
        [SerializeField]
        private AudioSource[] _defaultFootsteps;
        [SerializeField]
        private AudioSource[] _rockFootSteps;
        [SerializeField]
        private AudioSource[] _woodFootSteps;
        [SerializeField]
        private AudioSource[] _sandFootSteps;
        [SerializeField]
        private AudioSource[] _metalFootSteps;

        public AudioSource[] GetFor(GroundMaterial? material)
        {
            if (material == null)
            {
                return _defaultFootsteps;
            }

            switch (material.Value)
            {
                case GroundMaterial.Rock:
                    return _rockFootSteps;
                case GroundMaterial.Wood:
                    return _woodFootSteps;
                case GroundMaterial.Sand:
                    return _sandFootSteps;
                case GroundMaterial.Metal:
                    return _metalFootSteps;
                default:
                    return _defaultFootsteps;
            }
        }

        public AudioSource GetRandomFor(GroundMaterial? material)
        {
            var audioClips = GetFor(material);
            if (audioClips == null || audioClips.Length == 0)
            {
                return null;
            }
            return audioClips.Random();
        }
    }

    public class MaterialFootstepPlayer : MonoBehaviour
    {
        private GroundMaterial? _material;

        public Transform groundCheck;
        public LayerMask whatIsGround;
        public float groundCheckRadius = 0.2f;

        public AudioMaterialMap audioMaterial;
        public AudioMaterialMap landingAudioMaterial;

        public MaterialFootstepPlayer()
        {
            audioMaterial = new AudioMaterialMap();
        }

        public void PlayFootsteps()
        {
            var audioClip = audioMaterial.GetRandomFor(_material);
            if (audioClip != null)
            {
                audioClip.Play();
            }
        }

        public void PlayLandingSound()
        {
            var audioClip = landingAudioMaterial.GetRandomFor(_material);
            if (audioClip != null)
            {
                audioClip.Play();
            }
        }

        private void Update()
        {
            var groundComponent =
                Physics2D
                    .OverlapCircleAll(groundCheck.position, groundCheckRadius, whatIsGround)
                    .Where(c => !c.isTrigger)
                    .Select(c => c.GetComponent<GroundComponent>())
                    .FirstOrDefault(c => c != null);

            _material = groundComponent == null ? default(GroundMaterial?) : groundComponent.groundMaterial;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}
