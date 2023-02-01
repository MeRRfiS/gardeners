using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MeleeWeapon : MonoBehaviour
{
    [SerializeField] private float angleOffset = 90;
    [SerializeField] private Ease attackEase;
    [SerializeField] private float attackTime = 0.2f;
    [SerializeField] private bool isAttacking = false;
    [SerializeField] public CameraController camController;
    void Start()
    {
        DOTween.Init();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isAttacking)
        {
            StartCoroutine(AttackCoroutine());
            isAttacking = true;
            DOTween.To(() => angleOffset, x => angleOffset = x, -angleOffset, 0.2f).SetEase(attackEase).OnComplete(() =>
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
            GetComponent<SpriteRenderer>().flipX = !GetComponent<SpriteRenderer>().flipX;
        }
    }
}
