using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(NightmareHunterController))]
public class NightmareHunterGateLiftAnimationController : MonoBehaviour {
    private readonly int GateKock = Animator.StringToHash("Gate knock");

    private Animator _animator;
    private NightmareHunterController _controller;

    // Use this for initialization
    void Awake ()
    {
        _animator = GetComponent<Animator>();
        _controller = GetComponent<NightmareHunterController>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        _animator.SetBool(GateKock, _controller.HorizontalSpeed != 0);
    }
}
