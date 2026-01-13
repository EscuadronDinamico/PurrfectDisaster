using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [Header("Movement")]
    public float moveSpeed;

    public float groundDrag;
    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask WhatIsGround;
    bool grounded;

    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;
    Rigidbody rb;

    [SerializeField] private Transform visualesGato;

    [SerializeField] private Animator animatorGato;

    private void Start()
    {


        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    private void Update()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, WhatIsGround);

        MyInput();
        SpeedControl();

        if (grounded)
            rb.linearDamping = groundDrag;
        else
            rb.linearDamping = 0;
    }
    private void FixedUpdate()
    {
        MovePlayer();
    }
    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");


    }
    private void MovePlayer()
    {
        Vector3 adelante = orientation.forward;
        adelante.y = 0;
        Vector3 derecha = orientation.right;
        derecha.y = 0;
        moveDirection = adelante * verticalInput + derecha * horizontalInput;
        visualesGato.forward = new Vector3(orientation.forward.x, 0, orientation.forward.z);
        float velocidad = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z).magnitude;


        rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        animatorGato.SetFloat("velocidad", velocidad);

    }
    private void SpeedControl()
    {

        Vector3 flatVel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);


        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.linearVelocity = new Vector3(limitedVel.x, rb.linearVelocity.y, limitedVel.z);
        }
    }
}