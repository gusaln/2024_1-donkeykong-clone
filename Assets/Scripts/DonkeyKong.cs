using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DonkeyKong : MonoBehaviour
{
    public Animator animator;

    void Awake() {
        animator = GetComponent<Animator>();
    }

    public void PlayThrow() {
        animator.SetBool("isThrowing", true);
    }

    public void StopThrow() {
        animator.SetBool("isThrowing", false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
