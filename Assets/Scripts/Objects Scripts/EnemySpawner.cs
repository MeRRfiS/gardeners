using NavMeshPlus.Components;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private EnemyTypesEnum type;
    [SerializeField] private Enemy enemyPrefab;

    [SerializeField] private Slider healthSlider;
    [SerializeField] private GameObject itemPrefab;

    public EnemyTypesEnum Type
    {
        set { type = value; }
    }

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

    public void CreateItem()
    {
        ProceduralMapGenerator.spawners.Remove(gameObject);

        var itemModel = new ItemsModel((int)ItemIds.APieceOfForestRoot,
                                       TextController.items.ItemName[(int)ItemIds.APieceOfForestRoot],
                                       ItemsTypeEnum.Material);

        var itemObj = Instantiate(itemPrefab);
        itemObj.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>($"ItemMiniIcon/{itemModel.Index}");
        itemObj.transform.position = gameObject.transform.position;
        itemObj.GetComponent<TropItem>().Item = itemModel;
    }
}
