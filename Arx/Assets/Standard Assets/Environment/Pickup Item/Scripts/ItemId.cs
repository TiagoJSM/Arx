using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Standard_Assets.Environment.Pickup_Item.Scripts
{
    public class ItemId : MonoBehaviour
    {
        [SerializeField]
        private Guid _id;
        [SerializeField]
        private string _sceneName;
    }
}
