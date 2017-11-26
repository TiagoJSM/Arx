using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Standard_Assets.Characters.Enemies.Spider_Mine.Scripts
{
    [RequireComponent(typeof(SpiderMineAi))]
    [RequireComponent(typeof(Animator))]
    public class SpiderMineAnimationController : MonoBehaviour
    {
        private readonly int Velocity = Animator.StringToHash("Velocity");
        private readonly int Waiting = Animator.StringToHash("Waiting");
        private readonly int BlowUpCountdown = Animator.StringToHash("Blow up countdown");

        private SpiderMineAi _spiderMineAi;
        private Animator _animator;

        private void Awake()
        {
            _spiderMineAi = GetComponent<SpiderMineAi>();
            _animator = GetComponent<Animator>();
        }

        private void Update()
        {
            _animator.SetFloat(Velocity, 10);
            _animator.SetBool(Waiting, _spiderMineAi.Enemy == null);
            _animator.SetBool(BlowUpCountdown, _spiderMineAi.BlowUpCountdown);
        }
    }
}
