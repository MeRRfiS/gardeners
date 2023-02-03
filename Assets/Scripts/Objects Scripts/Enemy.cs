using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    private EnemiesModel _enemiesModel = new EnemiesModel();

    private NavMeshAgent _agent;

    private bool isHitPlayer = false;

    [SerializeField] private Slider health;
    [SerializeField] private GameObject itemPrefab;

    public EnemiesModel EnemiesModel
    {
        get { return _enemiesModel; }
        set { _enemiesModel = value; }
    }

    public NavMeshAgent Agent
    {
        get { return _agent; }
        set { _agent = value; }
    }

    private void Start()
    {
        health.maxValue = _enemiesModel.Lives;
        health.value = _enemiesModel.Lives;
        _agent = GetComponent<NavMeshAgent>();
        _agent.updateRotation = false;
        _agent.updateUpAxis = false;
        _agent.speed = _enemiesModel.Speed;
    }

    private void Update()
    {
        if (health.value == health.maxValue)
        {
            health.gameObject.SetActive(false);
        }
        else
        {
            health.gameObject.SetActive(true);
        }

        health.value = _enemiesModel.Lives;

        if (Vector2.Distance(PlayerController.GetInstance().Player.transform.position, 
                            gameObject.transform.position) <= 1.5f)
        {
            _agent.SetDestination(gameObject.transform.position);
            StartCoroutine(HitDamage());
        }
        else
        {
            isHitPlayer = false;
            StopCoroutine(HitDamage());
            _agent.SetDestination(PlayerController.GetInstance().Player.transform.position);
        }
    }

    private void OnDestroy()
    {
        List<ItemsModel> itemsModel = new List<ItemsModel>();

        switch (_enemiesModel.Type)
        {
            case EnemyTypesEnum.Spider:
                itemsModel.Add(new ItemsModel((int)ItemIds.Web,
                                           TextController.items.ItemName[(int)ItemIds.Web],
                                           ItemsTypeEnum.Material));
                break;
            case EnemyTypesEnum.Apple:
                itemsModel.Add(new ItemsModel((int)ItemIds.MonsterApplePits,
                                           TextController.items.ItemName[(int)ItemIds.MonsterApplePits],
                                           ItemsTypeEnum.Material));
                break;
            case EnemyTypesEnum.Ent:
                itemsModel.Add(new ItemsModel((int)ItemIds.TreeSap,
                                           TextController.items.ItemName[(int)ItemIds.TreeSap],
                                           ItemsTypeEnum.Material));
                break;
        }

        if (Random.Range(1, 101) <= (100 / (30 / _enemiesModel.Duration)))
        {
            itemsModel.Add(new ItemsModel((int)ItemIds.Heal,
                                           "",
                                           ItemsTypeEnum.Bonus));
        }

        foreach (var itemModel in itemsModel)
        {
            var itemObj = Instantiate(itemPrefab);
            itemObj.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>($"ItemMiniIcon/{itemModel.Index}");
            itemObj.transform.position = gameObject.transform.position;
            itemObj.GetComponent<TropItem>().Item = itemModel;
        }
    }

    private IEnumerator HitDamage()
    {
        if (!isHitPlayer)
        {
            isHitPlayer = true;
            while (isHitPlayer)
            {
                PlayerController.GetInstance().healthBar.value -= _enemiesModel.Damage;

                yield return new WaitForSeconds(0.5f);
            }
        }
    }
}
