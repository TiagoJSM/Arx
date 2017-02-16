using Assets.Standard_Assets.Environment.Teleports.Teleport_On_Use.Scripts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public interface ITeleporter
{
    TeleportOnUseNotification Notification { get; }
    void Teleport(GameObject teleportTarget);
}

