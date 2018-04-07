using Assets.Standard_Assets.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Standard_Assets.Environment.Hazards.Falling_Rock.Scripts
{
    public class FallingRockController : MonoBehaviour
    {
        private bool _played;

        [SerializeField]
        private GameObject _breakTarget;
        [SerializeField]
        private GameObject _fallingRock;
        [SerializeField]
        private float _duration = 1;
        [SerializeField]
        private AudioSource _rockBreaking;
        [SerializeField]
        private AudioSource _rockImpact;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!_played && collision.gameObject.IsPlayer())
            {
                _played = true;
                StartCoroutine(FallingRoutine());
            }
        }

        private IEnumerator FallingRoutine()
        {
            var breakTargetPosition = _breakTarget.transform.position;
            var fallingRockStartPosition = _fallingRock.transform.position;
            var fallingRockTransform = _fallingRock.transform;
            _rockBreaking.Play();
            var t = 0.0f;
            while (!_fallingRock.transform.position.ToVector2().Approximately(breakTargetPosition))
            {
                fallingRockTransform.position = MathUtilities.ExponentialInterpolation(fallingRockStartPosition, breakTargetPosition, _duration, ref t);
                yield return null;
            }
            
            _rockImpact.Play();
            fallingRockTransform.gameObject.SetActive(false);
        }
    }
}
