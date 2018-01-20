using Assets.Standard_Assets.Extensions;
using Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Standard_Assets.Characters.Enemies.Alarm_Bot.Scripts
{
    public class AlarmBotSounds : MonoBehaviour
    {
        private AudioSource _selectedMovementSound;

        [SerializeField]
        private AudioSource[] _iddleMovementSounds;
        [SerializeField]
        private AudioSource[] _alarmMovementSounds;
        [SerializeField]
        private AudioSource _alarmSounds;

        public void IddleMovement()
        {
            _alarmSounds.Stop();
            PlayMovementSound(_iddleMovementSounds);
        }

        public void AlarmMovement()
        {
            _alarmSounds.Play();
            PlayMovementSound(_alarmMovementSounds);
        }

        public void PlayMovementSound(AudioSource[] sounds)
        {
            if (_selectedMovementSound)
            {
                _selectedMovementSound.Stop();
            }

            _selectedMovementSound = sounds.Random();
            _selectedMovementSound.Play();
        }

        public void Death()
        {
            if (_selectedMovementSound)
            {
                StartCoroutine(_selectedMovementSound.FadeOut(1));
            }
            if (_alarmSounds.isPlaying)
            {
                StartCoroutine(_alarmSounds.FadeOut(1));
            }
        }
    }
}
