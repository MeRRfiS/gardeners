using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, IController
{
    private static PlayerController instance;

    private Vector2 moveDir;

    [Header("Components")]
    [SerializeField] private GameObject _player;
    [SerializeField] private Rigidbody2D rigidbody2D;

    [Header("Player Status")]
    [SerializeField] private float speed = 10f;

    private bool _isCanMove = true;

    public GameObject Player
    {
        get { return _player; }
    }

    public bool IsCanMove
    {
        set { _isCanMove = value; }
    }

    public LoadStatusEnum Status { get; private set; }

    public void StartUp()
    {
        instance = this;

        Status = LoadStatusEnum.IsLoaded;
    }

    public static PlayerController GetInstance()
    {
        return instance;
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
