using Assets.Standard_Assets._2D.Scripts.Controllers;
using CommonInterfaces.Controllers.Interaction;
using Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class InteractionFinder : MonoBehaviour
{
    private IInteractionTriggerController _trigger;

    [SerializeField]
    private InteractionNotification _notification;

    [SerializeField]
    private Transform _interactionAreaP1;
    [SerializeField]
    private Transform _interactionAreaP2;

    public IInteractionTriggerController GetInteractionTrigger()
    {
        var interactionTriggerColliders =
            Physics2D
                .OverlapAreaAll(_interactionAreaP1.position, _interactionAreaP2.position)
                .Select(c => c.GetComponent<IInteractionTriggerController>())
                .Where(c => c != null);

        if (!interactionTriggerColliders.Any())
        {
            return null;
        }

        var thisPosition = transform.position;
        var closestInteraction = interactionTriggerColliders.MinBy(c =>
            Vector2.Distance(thisPosition, c.GameObject.transform.position));

        return closestInteraction;
    }

    private void Update()
    {
        var closestInteraction = GetInteractionTrigger();

        if (_trigger != null && closestInteraction == null)
        {
            _trigger = null;
            _notification.HideInteraction();
        }
        else if(closestInteraction != null && (_trigger == null || closestInteraction != _trigger))
        {
            _trigger = closestInteraction;
            _notification.ShowInteraction();
        }
    }

    private void OnDrawGizmos()
    {
        if (_interactionAreaP1 == null || _interactionAreaP2 == null)
        {
            return;
        }
        Gizmos.color = Color.red;
        var center = (_interactionAreaP1.position + _interactionAreaP2.position) / 2;
        var size = _interactionAreaP1.position - _interactionAreaP2.position;
        size = new Vector3(Mathf.Abs(size.x), Mathf.Abs(size.y), Mathf.Abs(size.z));
        Gizmos.DrawWireCube(center, size);
    }
}

