using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class ShadowHandAnimationController : MonoBehaviour
{
    private readonly int GrabbingAnimation = Animator.StringToHash("Grabbing Animation");

    private Animator _animator;

    [SerializeField]
    [Range(0, 1)]
    private int _grabbingAnimation;

    void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    void Update()
    {
        _animator.SetInteger(GrabbingAnimation, _grabbingAnimation);
    }
}

