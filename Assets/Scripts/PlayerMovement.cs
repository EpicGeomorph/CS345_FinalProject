using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float speed = 4f;

    CharacterController controller;
    InputAction move;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        move = InputSystem.actions.FindAction("Move");
    }

    void Update()
    {
        if (DialogueBox.IsOpen) return;

        Vector2 input = move.ReadValue<Vector2>();
        Vector3 direction = new Vector3(input.x, 0f, input.y);
        controller.SimpleMove(direction * speed);
        if (direction != Vector3.zero) transform.forward = direction;
    }
}
