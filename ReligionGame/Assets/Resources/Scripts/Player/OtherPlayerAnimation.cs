using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OtherPlayerAnimation : MonoBehaviour
{
    public Transform groundCheck;
    public float groundDistance;
    public LayerMask groundMask;

    // Update is called once per frame
    void Update()
    {
        bool isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        GetComponent<PlayerManager>().animator.SetBool("isSprint", GetComponent<PlayerManager>().IsSprint);

        if (!isGrounded)
        {
            GetComponent<PlayerManager>().animator.SetBool("isGrounded", false);
            GetComponent<PlayerManager>().animator.SetFloat("velocityY", Mathf.Sign(transform.position.normalized.y));
        }

        if (isGrounded)
        {
            GetComponent<PlayerManager>().animator.SetBool("isGrounded", true);
            GetComponent<PlayerManager>().animator.SetFloat("velocityY", 0);
        }
    }
}
