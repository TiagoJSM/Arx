using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Standard_Assets._2D.Scripts.Controllers
{
    public class InteractiveDialogComponent : MonoBehaviour
    {
        [Speakers]
        [SerializeField]
        private InteractiveDialog _dialog;

        public int DialogsCount { get { return _dialog.DialogsCount; } }

        public InteractiveDialogContext GetDialogContext(int idx)
        {
            return _dialog.GetDialogContext(idx);
        }
    }
}
