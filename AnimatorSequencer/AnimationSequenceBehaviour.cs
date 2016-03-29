using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;

namespace AnimatorSequencer
{
    public class AnimationThread
    {
        private AnimationSequenceNode _node;

        public AnimationSequenceNode Node
        {
            get
            {
                return _node;
            }
        }

        public AnimationThread(AnimationSequenceNode node)
        {
            _node = node;
            _node.state.OnStateEnter();
        }

        public void FixedUpdate()
        {
            _node.state.OnStateFixedUpdate();
        }

        public void Update()
        {
            _node.state.OnStateUpdate();
        }

        public bool Complete()
        {
            return _node.state.Complete();
        }

        public void OnExit()
        {
            _node.state.OnStateExit();
        }
    }

    public class AnimationSequenceBehaviour : MonoBehaviour
    {
        private List<AnimationThread> _runningThreads;
        private bool _run;

        [SerializeField]
        [HideInInspector]
        public AnimationSequenceNode root;

        [HideInInspector]
        public AnimationSequenceNode clonedRoot;

        public AnimationSequenceBehaviour()
        {
        }

        void FixedUpdate()
        {
            //test.Invoke(1, 0, false);
            PerformUpdate(t => t.FixedUpdate());
        }

        void Update()
        {
            PerformUpdate(t => t.Update());
        }

        public void Run()
        {
            if (_run)
            {
                return;
            }
            AssignInitialAnimationThreads();
            _run = true;
        }

        private void AssignInitialAnimationThreads()
        {
            _runningThreads = clonedRoot.nextStates.Select(s => new AnimationThread(s)).ToList();
        }

        private void PerformUpdate(Action<AnimationThread> action)
        {
            if (!_run)
            {
                return;
            }

            var threadsCopy = _runningThreads.ToList();
            foreach (var thread in threadsCopy)
            {
                //thread.Update();
                action(thread);
                if (thread.Complete())
                {
                    thread.OnExit();
                    _runningThreads.Remove(thread);
                    _runningThreads.AddRange(thread.Node.nextStates.Select(s => new AnimationThread(s)));
                }
            }
        }
    }
}
