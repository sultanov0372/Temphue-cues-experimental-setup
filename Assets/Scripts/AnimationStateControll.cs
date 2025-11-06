using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationStateControll : MonoBehaviour
{
    public GameObject speechBubble;

    Animator animator;
    int isShakingHash;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        isShakingHash = Animator.StringToHash("hsStart");
    }

    // Update is called once per frame
    void Update()
    {
        //bool isShaking = animator.GetBool(isShakingHash);
        //bool kPressed = Input.GetKey("k");

        animator.SetBool(isShakingHash, Input.GetKey("k"));

        if (Input.GetKeyDown(KeyCode.K))
        {
            
        }
    }
}
