﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommonInterfaces.Inventory
{
    public interface IInventoryItem
    {
        string Name { get; }
        IItemOwner Owner { get; set; }
    }
}