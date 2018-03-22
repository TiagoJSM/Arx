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
        public const string EnemyTag = "Enemy";

        public static bool IsPlayer(this GameObject go)
        {
            return go.CompareTag(PlayerTag);
        }

        public static bool IsPlayer(this Collider2D collider)
        {
            return collider.gameObject.IsPlayer();
        }

        public static LayerMask GetLayerMask(this GameObject go)
        {
            return (1 << go.layer);
        }
    }
}
