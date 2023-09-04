using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public float rotationSpeed = 50f;
    public float jumpForce = 7f;
    public Transform groundCheck;
    public LayerMask groundMask;

    private Rigidbody rb;
    private bool isGrounded;

    private Animator animator;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {

        float movimientoHorizontal = Input.GetAxis("Horizontal");
        float movimientoVertical = Input.GetAxis("Vertical");

        Vector3 movementDirection = new Vector3(movimientoHorizontal, 0, movimientoVertical);

        if (movementDirection.sqrMagnitude > 0.001f)
        {
            movementDirection.Normalize();

            transform.position = transform.position + movementDirection * speed * Time.deltaTime;

            Quaternion targetRotation = Quaternion.LookRotation(movementDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    private void Update()
    {
        if (Physics.CheckSphere(groundCheck.position, 0.1f, groundMask))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }

        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            animator.SetTrigger("Jump");
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }
}
