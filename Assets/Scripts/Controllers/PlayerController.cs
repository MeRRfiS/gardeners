using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour, IController
{
    private static PlayerController instance;

    private Vector2 moveDir;

    private const float DEFAULT_SPEED = 10f;
    private const int DEFAULT_STRANGE = 0;
    private const int DEFAULT_HEATH = 1900;
    private const float DEFAULT_SPEED_ATTACK = 0.5f;

    [Header("Components")]
    [SerializeField] private GameObject _player;
    [SerializeField] private Rigidbody2D rigidbody2D;

    [Header("UI Components")]
    [SerializeField] private GameObject playerStatistic;
    public Slider healthBar;

    private bool _isCanMove = true;
    private bool hasKVZHP = false;

    public float Speed { get; set; }
    public int Damage { get; set; }
    public int Heath { get; set; }
    public float SpeedAttack { get; set; }

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
        playerStatistic.SetActive(false);

        Status = LoadStatusEnum.IsLoaded;
    }

    public static PlayerController GetInstance()
    {
        return instance;
    }

    public void SetValueToPlayer()
    {
        float bonusSpeed = 0;
        int bonusDamage = 0;
        int bonusHealth = 0;
        float bonusSpeedAttack = 0;

        Speed = DEFAULT_SPEED;
        Damage = DEFAULT_STRANGE;
        Heath = DEFAULT_HEATH;
        SpeedAttack = DEFAULT_SPEED_ATTACK;

        foreach (ItemsModel item in InventarController.GetInstance().itemSelect)
        {
            switch (item.Index)
            {
                case ((int)ItemIds.TechGloves):
                    bonusSpeedAttack -= 0.1f;
                    break;
                case ((int)ItemIds.SensoryBoots):
                    bonusSpeed += 1f;
                    break;
                case ((int)ItemIds.Blade):
                    bonusDamage += 10;
                    bonusHealth -= (int)(DEFAULT_HEATH * (10 / 2 / 100));
                    break;
                case ((int)ItemIds.NATOBulletproofVest):
                    bonusHealth += 100;
                    bonusSpeed -= 2f;
                    break;
                default:
                    break;
            }
        }

        if(InventarController.GetInstance().weaponSelect != null)
        {
            switch (InventarController.GetInstance().weaponSelect.Index)
            {
                case ((int)ItemIds.Axe):
                    bonusDamage += 20;
                    break;
                default:
                    break;
            }
        }

        Speed += bonusSpeed;
        Damage += bonusDamage;
        Heath += bonusHealth;
        SpeedAttack += bonusSpeedAttack;

        healthBar.maxValue = Heath;
        healthBar.value = Heath;
    }

    private void Update()
    {
        foreach (ItemsModel item in InventarController.GetInstance().itemSelect)
        {
            if (item.Index != (int)ItemIds.KVZHP)
            {
                hasKVZHP = false;
            }
            else
            {
                hasKVZHP = true;
                break;
            }
        }

        if (hasKVZHP)
        {
            playerStatistic.SetActive(true);
            Debug.Log($"{healthBar.value}|{Heath * 0.2f}");
            if(healthBar.value < (Heath * 0.2f))
            {
                SceneManager.LoadScene("Lobbi");
            }
        }
        else
        {
            playerStatistic.SetActive(false);
        }

        if (_isCanMove)
        {
            Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            moveDir = moveInput.normalized * DEFAULT_SPEED;
        }
    }

    private void FixedUpdate()
    {
        if (_isCanMove)
        {
            rigidbody2D.MovePosition(rigidbody2D.position + moveDir * Time.deltaTime);
        }
    }
}
