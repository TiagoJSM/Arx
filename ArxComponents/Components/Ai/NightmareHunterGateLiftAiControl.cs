using ArxGame.StateMachine;
using CommonInterfaces.Controls;
using GenericComponents.UserControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Extensions;
using UnityEngine.Experimental.Director;

namespace ArxGame.Components.Ai
{
    public interface INightmareHunterGateLiftAiControl
    {
        bool ReachedGate { get; }
        bool ReachedTarget { get; }

        void MoveToTarget();
        void StopMoving();
        void KnockGate();
        void AttackTarget();
        void MoveAway();
        void Die();
    }

    public class NightmareHunterGateLiftAiControl : PlatformerAICharacterControl, INightmareHunterGateLiftAiControl
    {
        private NightmareHunterGateAiStateManager _aiManager;

        public GameObject moveAwayObject;
        public GameObject gate;
        public GameObject target;
        public float targetTreshold = 1;
        public AnimationClip knockGateAnimation;

        public bool ReachedGate
        {
            get; private set;
        }

        public bool ReachedTarget
        {
            get; private set;
        }

        public void MoveToTarget()
        {
            MoveDirectlyTo(target.transform.position.ToVector2(), targetTreshold);
        }

        public void KnockGate()
        {
            AnimationController.PlayAnimation(knockGateAnimation);
            //throw new NotImplementedException();
        }

        public void AttackTarget()
        {
            //throw new NotImplementedException();
        }

        public void MoveAway()
        {
            MoveDirectlyTo(moveAwayObject.transform.position.ToVector2(), targetTreshold);
        }

        public void Die()
        {
            //throw new NotImplementedException();
        }

        void Start()
        {
            base.PerformStart();
            _aiManager = new NightmareHunterGateAiStateManager(this);
        }

        void FixedUpdate()
        {
            base.PerformFixedUpdate();
            _aiManager.Perform(null);
        }

        void OnCollisionEnter2D(Collision2D other)
        {
            Debug.Log(other.gameObject);
            if(other.gameObject == gate)
            {
                ReachedGate = true;
            }
            if (other.gameObject == target)
            {
                ReachedTarget = true;
            }
        }
    }
}
