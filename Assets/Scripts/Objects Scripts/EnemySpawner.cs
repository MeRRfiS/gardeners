using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private EnemyTypesEnum type;
    [SerializeField] private Enemy enemyPrefab;

    private void Start()
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
        

        StartCoroutine(ToSpawn(enemyModel));
    }

    private IEnumerator ToSpawn(EnemiesModel enemyModel)
    {
        while (true)
        {
            yield return new WaitForSeconds(enemyModel.Duration);

            var enemyObject = Instantiate(enemyPrefab.gameObject);

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

            enemyObject.transform.position = gameObject.transform.position;
            enemyObject.GetComponent<Enemy>().EnemiesModel = enemyModel;
        }
    }
}
