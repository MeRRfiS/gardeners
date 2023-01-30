using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, IController
{
    private Vector2 moveDir;

    [Header("Components")]
    [SerializeField] private Rigidbody2D rigidbody2D;

    [Header("Player Status")]
    [SerializeField] private float speed = 10f;

    private bool _isCanMove = true;

    public bool IsCanMove
    {
        get { return _isCanMove; }
        set { _isCanMove = value; }
    }
    public LoadStatusEnum Status { get; private set; }

    public void StartUp()
    {
        Status = LoadStatusEnum.IsLoaded;
    }

    private void Update()
    {
        if (_isCanMove)
        {
            Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            moveDir = moveInput.normalized * speed;
        }
    }

    private void FixedUpdate()
    {
        rigidbody2D.MovePosition(rigidbody2D.position + moveDir * Time.deltaTime);
    }
}
