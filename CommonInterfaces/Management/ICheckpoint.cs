using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CommonInterfaces.Management
{
    public interface ICheckpoint
    {
        Vector3 Position { get; }
    }
}
