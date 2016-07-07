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
using ArxGame.Components.AnimationControllers;

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

    [RequireComponent(typeof(MainPlatformerController))]
    [RequireComponent(typeof(PlatformerCharacterAnimationController))]
    public class NightmareHunterGateLiftAiControl : MonoBehaviour, IPlatformerAICharacterControl, INightmareHunterGateLiftAiControl
    {
        private NightmareHunterGateAiStateManager _aiManager;
        private PlatformerCharacterAnimationController _animationController;
        private MainPlatformerController _characterController;

        public GameObject moveAwayObject;
        public GameObject gate;
        public GameObject target;
        public float targetTreshold = 1;
        public AnimationClip knockGateAnimation;

        private Vector2? _moveToPosition;
        private float _treshold;

        protected PlatformerCharacterAnimationController AnimationController
        {
            get
            {
                return _animationController;
            }
        }
        protected MainPlatformerController CharacterController
        {
            get
            {
                return _characterController;
            }
        }

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

        public void MoveDirectlyTo(Vector2 position, float treshold)
        {
            _moveToPosition = position;
            _treshold = treshold;
        }

        public void StopMoving()
        {
            _moveToPosition = null;
        }

        protected void Start()
        {
            _characterController = GetComponent<MainPlatformerController>();
            _aiManager = new NightmareHunterGateAiStateManager(this);
            _animationController = GetComponent<PlatformerCharacterAnimationController>();
        }

        protected void FixedUpdate()
        {
            _aiManager.Perform(null);
            if (_moveToPosition == null)
            {
                return;
            }

            var currentPosition = this.transform.position.ToVector2();
            var distance = Vector2.Distance(currentPosition, _moveToPosition.Value);
            if (distance < _treshold)
            {
                _moveToPosition = null;
                return;
            }
            var xDifference = _moveToPosition.Value.x - currentPosition.x;
            _characterController.Move(xDifference, 0, false);
        }

        void OnCollisionEnter2D(Collision2D other)
        {
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
