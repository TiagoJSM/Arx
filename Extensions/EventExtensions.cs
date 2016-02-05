using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Extensions
{
    public static class EventExtensions
    {
        public static void CancelDuplicate(this Event e)
        {
            e.commandName = string.Empty;
        }
    }
}
