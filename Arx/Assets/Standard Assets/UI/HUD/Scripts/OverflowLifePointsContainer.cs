using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Standard_Assets.UI.HUD.Scripts
{
    public class OverflowLifePointsContainer : MonoBehaviour
    {
        private int _lifePoints;

        [SerializeField]
        private Text _lifePointsText;

        public int LifePoints
        {
            get
            {
                return _lifePoints;
            }
            set
            {
                _lifePoints = value;
                _lifePointsText.text = "x " + _lifePoints;
            }
        }
    }
}
