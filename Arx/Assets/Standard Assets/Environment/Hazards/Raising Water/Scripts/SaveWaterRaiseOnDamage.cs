using Assets.Standard_Assets._2D.Scripts;
using Assets.Standard_Assets._2D.Scripts.Game_State;
using Assets.Standard_Assets._2D.Scripts.Managers;
using Assets.Standard_Assets.Scripts;
using CommonInterfaces.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Standard_Assets.Environment.Hazards.Raising_Water.Scripts
{
    public class SaveWaterRaiseOnDamage : MonoBehaviour
    {
        [SerializeField]
        private DummyCharacter _dummyCharacter;

        private void Awake()
        {
            _dummyCharacter.OnAttacked += OnAttackedHandler;
        }

        private void OnAttackedHandler(GameObject attacker, int damage, Vector3? hitPoint, DamageType damageType, AttackTypeDetail attackType, int comboNumber)
        {
            var watershed = GameStateManager.Instance.GameState.Watershed.WaterLevelChanged = true;
        }
    }
}
