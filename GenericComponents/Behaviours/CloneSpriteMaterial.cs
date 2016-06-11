using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace GenericComponents.Behaviours
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class CloneSpriteMaterial : MonoBehaviour
    {
        void Awake()
        {
            var sprite = GetComponent<SpriteRenderer>();
            sprite.material = new Material(sprite.material);
        }
    }
}
