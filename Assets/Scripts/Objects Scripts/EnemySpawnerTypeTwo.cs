using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerTypeTwo : MonoBehaviour
{
    [SerializeField] private EnemyTypesEnum type;
    [SerializeField] private Enemy enemyPrefab;

    public EnemyTypesEnum Type
    {
        set { type = value; }
    }

    public int Health { get; set; }

    private void Start()
    {
        StartCoroutine(ToSpawn());
    }

    private IEnumerator ToSpawn()
    {
        while (true)
        {
            EnemiesModel enemyModel = new EnemiesModel();

            switch (type)
            {
                case EnemyTypesEnum.Spider:
                    enemyModel = new EnemiesModel(200, 3.5f, 5, 3.3f * 6, type);
                    break;
                case EnemyTypesEnum.Apple:
                    enemyModel = new EnemiesModel(100, 10f, 1, 1.8f * 6, type);
                    break;
                case EnemyTypesEnum.Ent:
                    enemyModel = new EnemiesModel(500, 1.5f, 20, 10f * 6, type);
                    break;
            }

            yield return new WaitForSeconds(enemyModel.Duration);

            var enemyObject = Instantiate(enemyPrefab.gameObject);
            enemyObject.transform.position = new Vector2(gameObject.transform.position.x,
                                                         gameObject.transform.position.y - 1.5f);
            enemyObject.GetComponent<Enemy>().EnemiesModel = enemyModel;

            switch (enemyModel.Type)
            {
                case EnemyTypesEnum.Spider:
                    enemyObject.GetComponent<SpriteRenderer>().sprite =
                        Resources.Load<Sprite>($"Enemy/{(int)enemyModel.Type}");
                    enemyObject.transform.localScale = new Vector3(1.25f, 1.25f);
                    break;
                case EnemyTypesEnum.Apple:
                    enemyObject.GetComponent<SpriteRenderer>().sprite =
                        Resources.Load<Sprite>($"Enemy/{(int)enemyModel.Type}");
                    enemyObject.GetComponent<Animator>().runtimeAnimatorController =
                        Resources.Load<RuntimeAnimatorController>($"Enemy Animation/{(int)enemyModel.Type}");
                    break;
                case EnemyTypesEnum.Ent:
                    enemyObject.GetComponent<SpriteRenderer>().sprite =
                        Resources.Load<Sprite>($"Enemy/{(int)enemyModel.Type}");
                    enemyObject.transform.localScale = new Vector3(2f, 2f);
                    break;
            }
        }
    }
}
