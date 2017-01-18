using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Standard_Assets.UI.HUD.Scripts
{
    public class CharacterStatusManager : MonoBehaviour
    {
        private int _health;

        [SerializeField]
        private Image _healthBar;

        public int MaxHealth { get; set; }
        public int Health
        {
            get
            {
                return _health;
            }
            set
            {
                _health = Mathf.Clamp(value, 0, MaxHealth);
                var healthBarScale = _healthBar.transform.localScale;
                _healthBar.transform.localScale = new Vector3(_health / MaxHealth, healthBarScale.y, healthBarScale.z);
            }
        }
    }
}
