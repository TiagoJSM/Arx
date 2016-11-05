using CommonInterfaces.Controllers.Interaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class InteractionFinder : MonoBehaviour
{
    [SerializeField]
    private Transform _interactionAreaP1;
    [SerializeField]
    private Transform _interactionAreaP2;

    public IInteractionTriggerController GetInteractionTrigger()
    {
        var interactionTriggerCollider =
            Physics2D
                .OverlapAreaAll(_interactionAreaP1.position, _interactionAreaP2.position)
                .FirstOrDefault(c => c.GetComponent<IInteractionTriggerController>() != null);
        
        if(interactionTriggerCollider == null)
        {
            return null;
        }
        return interactionTriggerCollider.GetComponent<IInteractionTriggerController>();
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

