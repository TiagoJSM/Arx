using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Standard_Assets.Extensions
{
    public static class GameObjectExtensions
    {
        public const string PlayerTag = "Player";

        public static bool IsPlayer(this GameObject go)
        {
            return go.CompareTag(PlayerTag);
        }
    }
}
