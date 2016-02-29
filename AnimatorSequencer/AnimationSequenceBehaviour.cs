using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace AnimatorSequencer
{
    [Serializable]
    public class StateParameters
    {
        [SerializeField]
        public BaseSequenceState state;
        [SerializeField]
        public string parameterName;
        [SerializeField]
        public UnityEngine.Object value;
    }

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
        public List<StateParameters> stateParameters;

        public AnimationSequenceNode root;
        
        public AnimationSequenceBehaviour()
        {
            stateParameters = new List<StateParameters>();
        }

        void Update()
        {
            if (!_run)
            {
                return;
            }

            var threadsCopy = _runningThreads.ToList();
            foreach (var thread in threadsCopy)
            {
                thread.Update();
                if (thread.Complete())
                {
                    thread.OnExit();
                    _runningThreads.Remove(thread);
                    _runningThreads.AddRange(thread.Node.nextStates.Select(s => new AnimationThread(s)));
                }
            }
        }

        public void Run()
        {
            if (_run)
            {
                return;
            }
            BindParameters();
            AssignInitialAnimationThreads();
            _run = true;
        }

        public StateParameters GetParameter(BaseSequenceState state, string parameterName)
        {
            var stateParameter = stateParameters.FirstOrDefault(sp => sp.state == state && sp.parameterName == parameterName);
            if (stateParameter != null)
            {
                return stateParameter;
            }
            return SetParameters(state, parameterName, null);
        }

        public StateParameters SetParameters(BaseSequenceState state, string parameterName, UnityEngine.Object value)
        {
            var stateParameter = stateParameters.FirstOrDefault(sp => sp.state == state && sp.parameterName == parameterName);
            if(stateParameter != null)
            {
                stateParameter.value = value;
                return stateParameter;
            }

            stateParameter = new StateParameters()
            {
                state = state,
                parameterName = parameterName,
                value = value
            };
            stateParameters.Add(stateParameter);
            return stateParameter;
        }

        private void BindParameters()
        {
            foreach (var stateParameter in stateParameters)
            {
                stateParameter.state.GetType().GetField(stateParameter.parameterName).SetValue(stateParameter.state, stateParameter.value);
            }
        }

        private void AssignInitialAnimationThreads()
        {
            _runningThreads = root.nextStates.Select(s => new AnimationThread(s)).ToList();
        }
    }
}
