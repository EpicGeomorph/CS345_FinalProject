using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerHandler : MonoBehaviour
{
    [SerializeField] public float movespeed = 5f;
    private Animator animator;
    private Rigidbody2D rigidbody;
    private SpriteRenderer sprite_renderer;
    private Vector2 direction;
    public bool currently_chatting = false;

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


        // Find Closest NPC
        float closestDistance = Mathf.Infinity;
        GameObject closestNPC = null;
        GameObject npcs = GameObject.Find("NPCs");

        foreach (Transform npc in npcs.transform)
        {
            float distance = Vector3.Distance(
                transform.position,
                npc.position
            );

            if (distance < closestDistance && distance < 1.5)
            {
                closestDistance = distance;
                closestNPC = npc.gameObject;
            }
        }

        if (closestNPC != null)
        {
            DialogueHandler npc_dialogue = closestNPC.GetComponent<DialogueHandler>();
            if (Input.GetKeyDown(KeyCode.E) && npc_dialogue != null && !currently_chatting)
            {
                currently_chatting = true;
                npc_dialogue.InitDialogue();
            }
        }
    }
}
