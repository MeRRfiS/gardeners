using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private EnemiesModel _enemiesModel = new EnemiesModel();

    private NavMeshAgent _agent;

    private bool isHitPlayer = false;

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
        _agent = GetComponent<NavMeshAgent>();
        _agent.updateRotation = false;
        _agent.updateUpAxis = false;
        _agent.speed = _enemiesModel.Speed;
    }

    private void Update()
    {
        _agent.SetDestination(PlayerController.GetInstance().Player.transform.position);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name != "Player") return;

        isHitPlayer = true;
        StartCoroutine(HitDamage());
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.name != "Player") return;

        isHitPlayer = false;
        StopCoroutine(HitDamage());
    }

    private IEnumerator HitDamage()
    {
        while (isHitPlayer)
        {
            PlayerController.GetInstance().healthBar.value -= _enemiesModel.Damage;

            yield return new WaitForSeconds(0.5f);
        }
    }
}
