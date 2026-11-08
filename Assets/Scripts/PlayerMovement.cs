using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] public float movespeed = 5f;
    private Animator animator;
    private Rigidbody2D rigidbody;
    private SpriteRenderer sprite_renderer;
    private Vector2 direction;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody2D>();
        sprite_renderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        // Reads user input, so GetAxis(Horizontal) will return 1,0,-1 depending on the keys pressed (A = -1, D = 1, nothing = 0)
        direction = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        rigidbody.linearVelocity = direction * movespeed;


        if (Input.GetAxis("Horizontal") > 0)
        {
            sprite_renderer.flipX = true;
        } else if (Input.GetAxis("Horizontal") < 0)
        {
            sprite_renderer.flipX = false;
        }

        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            animator.SetBool("Walking", true);
        } else
        {
            animator.SetBool("Walking", false);
        }
    }
}
