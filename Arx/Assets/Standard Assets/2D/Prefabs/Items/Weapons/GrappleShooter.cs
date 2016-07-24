using UnityEngine;
using System.Collections;
using ArxGame.Components.Weapons;
using CommonInterfaces.Enums;

[RequireComponent(typeof(HingeJoint2D))]
public class GrappleShooter : MonoBehaviour {
    private Grapple _instanciatedGrapple;
    private HingeJoint2D _hingeJoint;

    public Grapple grapplePrefab;
	// Use this for initialization
	void Start () {
        _hingeJoint = GetComponent<HingeJoint2D>();
        _instanciatedGrapple = Instantiate(grapplePrefab);
        _instanciatedGrapple.transform.parent = this.transform;
        _instanciatedGrapple.transform.localPosition = Vector3.zero;
        _instanciatedGrapple.origin = _hingeJoint;
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("Fire1"))
        {
            _instanciatedGrapple.Throw(Direction.Right);
        }
	}
}
