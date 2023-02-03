using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private EnemyTypesEnum type;
    [SerializeField] private Enemy enemyPrefab;

    [SerializeField] private Slider healthSlider;

    public int Health { get; set; }

    private void Start()
    {
        Health = 1000;
        healthSlider.maxValue = Health;
        healthSlider.value = Health;

        switch (type)
        {
            case EnemyTypesEnum.Spider:
                GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Spawner Sprite/Portal Root");
                break;
            case EnemyTypesEnum.Apple:
                GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Spawner Sprite/Apple Tree");
                break;
            case EnemyTypesEnum.Ent:
                GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Spawner Sprite/Portal Root");
                break;
        }

        gameObject.AddComponent<PolygonCollider2D>();

        StartCoroutine(ToSpawn());
    }

    private void Update()
    {
        if(healthSlider.value != healthSlider.maxValue)
        {
            healthSlider.gameObject.SetActive(true);
        }

        healthSlider.value = Health;
    }

    private IEnumerator ToSpawn()
    {
        while (true)
        {
            EnemiesModel enemyModel = new EnemiesModel();

            switch (type)
            {
                case EnemyTypesEnum.Spider:
                    enemyModel = new EnemiesModel(200, 3.5f, 5, 3.3f, type);
                    break;
                case EnemyTypesEnum.Apple:
                    enemyModel = new EnemiesModel(100, 10f, 1, 1.8f, type);
                    break;
                case EnemyTypesEnum.Ent:
                    enemyModel = new EnemiesModel(500, 1.5f, 20, 10f, type);
                    break;
            }

            yield return new WaitForSeconds(enemyModel.Duration);

            var enemyObject = Instantiate(enemyPrefab.gameObject);
            enemyObject.transform.position = new Vector2(gameObject.transform.position.x,
                                                         gameObject.transform.position.y - 2.5f);
            enemyObject.GetComponent<Enemy>().EnemiesModel = enemyModel;

            switch (enemyModel.Type)
            {
                case EnemyTypesEnum.Spider:
                    enemyObject.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0);
                    break;
                case EnemyTypesEnum.Apple:
                    enemyObject.GetComponent<SpriteRenderer>().color = new Color(0.5f, 0, 0);
                    break;
                case EnemyTypesEnum.Ent:
                    enemyObject.GetComponent<SpriteRenderer>().color = new Color(0, 0.5f, 0);
                    break;
            }
        }
    }
}
