using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace GenericComponents.UI
{
    [RequireComponent(typeof(Text))]
    public class CloneUiTextMaterial : MonoBehaviour
    {
        void Awake()
        {
            var text = GetComponent<Text>();
            text.material = new Material(text.material);
        }
    }
}
