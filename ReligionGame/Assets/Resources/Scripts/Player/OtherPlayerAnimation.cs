using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OtherPlayerAnimation : MonoBehaviour
{
    public Transform groundCheck;
    public float groundDistance;
    public LayerMask groundMask;

    public PlayerManager curPlayer;

    private float x;
    private float z;

    private void Start()
    {
        curPlayer = GetComponent<PlayerManager>();
    }

    // Update is called once per frame
    void Update()
    {
        bool isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        curPlayer.animator.SetBool("isSprint", GetComponent<PlayerManager>().IsSprint);

        curPlayer.animator.SetFloat("horizontal", x);
        curPlayer.animator.SetFloat("vertical", z);

        if (!isGrounded)
        {
            curPlayer.animator.SetBool("isGrounded", false);
            curPlayer.animator.SetFloat("velocityY", Mathf.Sign(transform.position.normalized.y));
        }

        if (isGrounded)
        {
            curPlayer.animator.SetBool("isGrounded", true);
            curPlayer.animator.SetFloat("velocityY", 0);
        }
    }

    public void setPosCheck(Vector3 _posCheck)
    {
        if (_posCheck.x != _posCheck.z)
        {
            x = _posCheck.x;
            z = _posCheck.z;
        }
        else
        {
            x = z = 0;
        }
    }
}
