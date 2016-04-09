using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CommonInterfaces.Controls
{
    public interface IPlatformerAICharacterControl
    {
        void MoveDirectlyTo(Vector2 position, float treshold);
    }
}
