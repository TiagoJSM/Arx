using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace AnimatorSequencer
{
    [Serializable]
    public class BehaviourField
    {
        [SerializeField]
        private StateMachineBehaviour _stateMachineBehaviour;
        [SerializeField]
        private string _fieldName;
        [SerializeField]
        private UnityEngine.Object _value;
        [SerializeField]
        private int _order;

        public StateMachineBehaviour StateMachineBehaviour
        {
            get { return _stateMachineBehaviour; }
            set { _stateMachineBehaviour = value; }
        }
        public string FieldName
        {
            get { return _fieldName; }
            set { _fieldName = value; }
        }
        public UnityEngine.Object Value
        {
            get { return _value; }
            set { _value = value; }
        }
        public int Order
        {
            get { return _order; }
            set { _order = value; }
        }
    }
}
