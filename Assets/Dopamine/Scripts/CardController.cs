using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardController : MonoBehaviour
{
    [HideInInspector] public Animator anim;

    [SerializeField] private float speed = 5f;

    [SerializeField] private bool canMove;

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        if (canMove)
        {
            Movement();
        }

        InýtAnimation();
    }

    private void Movement()
    {
        transform.position += -transform.forward * Time.deltaTime * speed;
    }

    private void InýtAnimation()
    {
        if (canMove)
        {
            anim.SetBool("Walk", true);
        }
        else
        {
            anim.SetBool("Walk", false);
        }
    }
}
