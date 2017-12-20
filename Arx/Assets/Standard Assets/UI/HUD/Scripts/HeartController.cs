using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Standard_Assets.UI.HUD.Scripts
{
    public enum HeartSide
    {
        Left,
        Right
    }

    public class HeartController : MonoBehaviour
    {
        [SerializeField]
        private Animator _leftHeartAnimator;
        [SerializeField]
        private Animator _rightHeartAnimator;

        public void Lose(HeartSide side)
        {
            if (side == HeartSide.Left)
            {
                _leftHeartAnimator.SetTrigger("Lose");
            }
            else
            {
                _rightHeartAnimator.SetTrigger("Lose");
            }
        }

        public void Gain(HeartSide side)
        {
            if (side == HeartSide.Left)
            {
                _leftHeartAnimator.SetTrigger("Gain");
            }
            else
            {
                _rightHeartAnimator.SetTrigger("Gain");
            }
        }
    }
}
