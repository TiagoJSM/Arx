using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Extensions;

namespace Assets.Standard_Assets.Environment.Platforms.Raising_Pillar.Scripts
{
    public class PillarButtonController : MonoBehaviour
    {
        [SerializeField]
        private LayerMask _mask;
        [SerializeField]
        private RaisingPillar _pillar;
        [SerializeField]
        private Sprite _unpressed;
        [SerializeField]
        private Sprite _pressed;
        [SerializeField]
        private SpriteRenderer _renderer;

        private void Awake()
        {
            _pillar.OnRiseComplete += OnRiseCompleteHandler;
            _renderer.sprite = _unpressed;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (_mask.IsInAnyLayer(collision.gameObject))
            {
                _renderer.sprite = _pressed;
                _pillar.Raise();
            }
        }

        private void OnRiseCompleteHandler(RaisingPillar pillar)
        {
            _renderer.sprite = _unpressed;
        }
    }
}
