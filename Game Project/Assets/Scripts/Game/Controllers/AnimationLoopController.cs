using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationLoopController : MonoBehaviour
{
    private Animator animator;
    private int loopCount = 0;
    [SerializeField] private int maxLoops = 3; 

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= (loopCount + 1))
        {
            loopCount++;

            if (loopCount >= maxLoops)
            {
                Object.Destroy(gameObject);
            }
        }
    }
}
