using GenericComponents.Behaviours;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Assets.Standard_Assets.Ropes.Editors
{
    public class RopeWizard : ScriptableWizard
    {
        private const string RopeLayer = "Rope";

        public float partsCount = 6;
        public GameObject ending1;
        public GameObject ending2;
        public float colliderWidth = 2;

        [MenuItem("GameObject/2D Object/Create Rope")]
        private static void CreateWizard()
        {
            var wizard = ScriptableWizard.DisplayWizard<RopeWizard>("Create Rope", "Create", "Apply");
            wizard.helpString = "Set rope endings and attributes";
            
            var selectedObjects = Selection.transforms;
            if (selectedObjects.Length > 0)
            {
                wizard.ending1 = selectedObjects[0].gameObject;
            }
            if (selectedObjects.Length > 1)
            {
                wizard.ending2 = selectedObjects[1].gameObject;
            }

            wizard.isValid = IsValid(wizard.ending1, wizard.ending2, wizard.partsCount, wizard.colliderWidth);
        }

        private void OnWizardCreate()
        {
            var ropeGO = new GameObject("Rope");

            var top = ending1;
            var bottom = ending2;
            if (ending1.transform.position.y < ending2.transform.position.y)
            {
                top = ending2;
                bottom = ending1;
            }

            ropeGO.transform.position = top.transform.position;

            var distance = Vector3.Distance(top.transform.position, bottom.transform.position);
            var heading = bottom.transform.position - top.transform.position;
            var direction = heading / distance;
            var ropePartLength = distance / partsCount;
            var ropePart = default(GameObject);
            var previousJoint = default(Joint2D);

            for (var idx = 0; idx <= partsCount; idx++)
            {
                ropePart = new GameObject("Rope part " + idx);
                ropePart.layer = LayerMask.NameToLayer(RopeLayer);
                ropePart.transform.SetParent(ropeGO.transform, false);
                ropePart.transform.localPosition = direction * idx * ropePartLength;
                if(idx == 0)
                {
                    previousJoint = ropePart.AddComponent<FixedJoint2D>();
                }
                else
                {
                    var joint = ropePart.AddComponent<HingeJoint2D>();
                    joint.connectedBody = previousJoint.GetComponent<Rigidbody2D>();
                    previousJoint = joint;
                }

                //not last one
                if(idx != partsCount)
                {
                    var box = ropePart.AddComponent<BoxCollider2D>();
                    box.offset = new Vector2(0, -(ropePartLength / 2));
                    box.size = new Vector2(colliderWidth, ropePartLength);
                }
            }

            var rope = ropeGO.AddComponent<Rope>();
            var renderer = ropeGO.AddComponent<RopeRenderer>();
            var lineRenderer = renderer.GetComponent<LineRenderer>();
            lineRenderer.useWorldSpace = false;
            renderer.Rope = rope;
            rope.RopeEnd = previousJoint;
        }

        private void OnWizardUpdate()
        {
            isValid = IsValid(ending1, ending2, partsCount, colliderWidth);
        }

        // When the user pressed the "Apply" button OnWizardOtherButton is called.
        private void OnWizardOtherButton()
        {
            
        }

        private static bool IsValid(GameObject ending1, GameObject ending2, float partsCount, float colliderWidth)
        {
            return
               ending1 != null &&
               ending2 != null &&
               partsCount > 0 &&
               colliderWidth > 0;
        }
    }
}
