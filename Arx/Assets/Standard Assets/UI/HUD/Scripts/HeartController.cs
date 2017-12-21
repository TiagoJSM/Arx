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
            var animator = side == HeartSide.Left ? _leftHeartAnimator : _rightHeartAnimator;
            animator.SetTrigger("Lose");
        }

        public void Gain(HeartSide side)
        {
            var animator = side == HeartSide.Left ? _leftHeartAnimator : _rightHeartAnimator;
            animator.SetTrigger("Gain");
        }

        public void SetFull(HeartSide side)
        {
            var animator = side == HeartSide.Left ? _leftHeartAnimator : _rightHeartAnimator;
            animator.PlayInFixedTime("Gain", -1, 1.0f);
        }

        public void SetEmpty(HeartSide side)
        {
            var animator = side == HeartSide.Left ? _leftHeartAnimator : _rightHeartAnimator;
            animator.PlayInFixedTime("Lose", -1, 1.0f);
        }
    }
}
