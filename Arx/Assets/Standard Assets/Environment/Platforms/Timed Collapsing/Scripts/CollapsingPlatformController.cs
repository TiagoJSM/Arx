using Assets.Standard_Assets.Common;
using Assets.Standard_Assets.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Standard_Assets.Environment.Platforms.Timed_Collapsing.Scripts
{
    public class CollapsingPlatformController : MonoBehaviour
    {
        private bool _awaitPlayer = true;

        [SerializeField]
        private GameObject _platform;
        [SerializeField]
        private float _collapsingTime = 0.5f;
        [SerializeField]
        private float _resetTime = 3.0f;
        [SerializeField]
        private float _vibrationValue = 0.2f;
        [SerializeField]
        private float _vibrationDuration = 0.3f;
        [SerializeField]
        private float _vibrationInterval = 0.05f;

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (_awaitPlayer && collision.gameObject.IsPlayer())
            {
                StartCoroutine(CollapseRoutine());
            }
        }

        private IEnumerator CollapseRoutine()
        {
            _awaitPlayer = false;
            yield return new WaitForSeconds(_collapsingTime);
            yield return CoroutineHelpers.Vibrate(_vibrationDuration, _vibrationValue, _vibrationInterval, _platform.transform);
            _platform.SetActive(false);
            yield return new WaitForSeconds(_resetTime);
            _platform.SetActive(true);
            _awaitPlayer = true;
        }
    }
}
