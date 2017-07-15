using Assets.Standard_Assets._2D.Scripts.Game_State;
using Assets.Standard_Assets._2D.Scripts.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Standard_Assets.Environment.Hazards.Raising_Water.Scripts
{
    public class RaiseWaterToTop : MonoBehaviour
    {
        [SerializeField]
        private Animator _animator;
        [SerializeField]
        private GameObject _disableWater;

        private void Awake()
        {
            if (GameStateManager.Instance.GameState.Watershed.WaterLevelChanged)
            {
                _animator.enabled = true;
                _animator.Play(_animator.GetCurrentAnimatorStateInfo(0).fullPathHash, -1, 1);
                _disableWater.SetActive(false);
            }
        }
    }
}
