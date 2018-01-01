using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Standard_Assets.Characters.CharacterBehaviour
{
    public interface ICharacterAware
    {
        void Aware(GameObject obj);
    }
}
