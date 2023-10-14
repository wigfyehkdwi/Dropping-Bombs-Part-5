using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketAnimate : MonoBehaviour
{
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        float horizontal = Input.GetAxis("Horizontal");

        animator.SetFloat("HAxis", horizontal);
    }
}
