using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Standard_Assets.Environment.Teleports.Teleport_On_Use.Scripts;
using UnityEngine;

public class TeleportToPositionOnUse : BaseTeleportToPosition, ITeleporter
{
    [SerializeField]
    private TeleportOnUseNotification _notification;

    public TeleportOnUseNotification Notification
    {
        get
        {
            return _notification;
        }
    }
}

