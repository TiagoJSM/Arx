using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Standard_Assets.Characters.Enemies.Alarm_Bot.Scripts
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(AlarmBotAiController))]
    public class AlarmBotAnimator : MonoBehaviour
    {
        private readonly int _warningId = Animator.StringToHash("Warning");
        private readonly int _deadID = Animator.StringToHash("Dead");
        private Animator _animator;
        private AlarmBotAiController _controller;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _controller = GetComponent<AlarmBotAiController>();
        }

        private void Update()
        {
            _animator.SetBool(_warningId, _controller.Warning);
            _animator.SetBool(_deadID, _controller.Dead);
        }
    }
}
