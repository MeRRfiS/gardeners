using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Vector2 moveDir;

    [Header("Components")]
    [SerializeField] private Rigidbody2D rigidbody2D;

    [Header("Player Status")]
    [SerializeField] private float speed = 10f;

    private void Start()
    {
        
    }

    private void Update()
    {
        Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        moveDir = moveInput.normalized * speed;
    }

    private void FixedUpdate()
    {
        rigidbody2D.MovePosition(rigidbody2D.position + moveDir * Time.deltaTime);
    }
}
