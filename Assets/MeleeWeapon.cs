using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MeleeWeapon : MonoBehaviour
{
    [SerializeField] private float angleOffset = 90;
    [SerializeField] private Ease attackEase;
    private float attackTime;
    [SerializeField] private bool isAttacking = false;
    [SerializeField] public CameraController camController;

    void Start()
    {
        DOTween.Init();
    }

    // Update is called once per frame
    void Update()
    {
        attackTime = PlayerController.GetInstance().SpeedAttack;

        if (Input.GetMouseButton(0) && !isAttacking)
        {
            StartCoroutine(AttackCoroutine());
            isAttacking = true;
i            DOTween.To(() => angleOffset, x => angleOffset = x, -angleOffset, attackTime).SetEase(attackEase).OnComplete(() =>
            isAttacking = false
            );
        }
        gameObject.transform.eulerAngles = new Vector3(0,0,angleOffset);
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 5.23f;

        Vector3 objectPos = Camera.main.WorldToScreenPoint(transform.position);
        mousePos.x = mousePos.x - objectPos.x;
        mousePos.y = mousePos.y - objectPos.y;

        float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle+angleOffset-90));

        IEnumerator AttackCoroutine()
        {
            yield return new WaitForSeconds(attackTime/2);
            HitDamage();
            GetComponent<SpriteRenderer>().flipX = !GetComponent<SpriteRenderer>().flipX;
        }
    }

    private void HitDamage()
    {
        var hits = Physics2D.CircleCastAll(PlayerController.GetInstance().Player.transform.position,
                                     2,
                                     PlayerController.GetInstance().Player.transform.position,
                                     0);

        foreach (RaycastHit2D hit in hits)
        {
            if(hit.collider.tag == "Enemy")
            {
                Enemy enemy = hit.collider.GetComponent<Enemy>();
                enemy.EnemiesModel.Lives -= PlayerController.GetInstance().Damage;
                if(enemy.EnemiesModel.Lives <= 0)
                {
                    Destroy(enemy.gameObject);
                }
            }
            else if(hit.collider.tag == "Spawner")
            {
                EnemySpawner spawner = hit.collider.GetComponent<EnemySpawner>();
                spawner.Health -= PlayerController.GetInstance().Damage;
                if (spawner.Health <= 0)
                {
                    Destroy(spawner.gameObject);
                }
            }
        }
    }
}
