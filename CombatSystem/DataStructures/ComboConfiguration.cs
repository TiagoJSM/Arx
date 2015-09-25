using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CombatSystem.Interfaces;

namespace CombatSystem.DataStructures
{
    public class ComboConfiguration
    {
        public ComboPreCondition PreCondition { get; set; }
        public float DurationMilliseconds { get; set; }
        public List<IActionConfiguraton> Actions { get; set; }
        public ComboConfiguration NextCombo { get; set; }
        public string AnimationName { get; set; }
        public string SoundName { get; set; }
        public bool CancelIfAttacked { get; set; }
        public float NextComboTriggeredBefore { get; set; }
        public OnStart OnStart { get; set; }
        public OnEnd OnEnd { get; set; }
        public OnCancelled OnCancelled { get; set; }
        public float NextComboTriggeredBeforeTotalTime
        {
            get
            {
                return (DurationMilliseconds * NextComboTriggeredBefore);    
            }
        }

        public ComboConfiguration()
        {
            Actions = new List<IActionConfiguraton>();
        }

        public bool CanTriggerNextCombo(float elapsedTimeMilliseconds)
        {
            var comboTriggerTime = DurationMilliseconds - NextComboTriggeredBeforeTotalTime;
            if (elapsedTimeMilliseconds < comboTriggerTime)
            {
                return false;
            }
            if (elapsedTimeMilliseconds > DurationMilliseconds)
            {
                return false;
            }
            return true;
        }
    }
}
