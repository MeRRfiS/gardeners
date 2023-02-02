public class EnemiesModel
{
    public int Lives { get; set; }
    public float Speed { get; set; }
    public int Damage { get; set; }
    public float Duration { get; set; }
    public EnemyTypesEnum Type { get; set; }

    public EnemiesModel()
    {
        Lives = 10;
        Speed = 0;
        Damage = 0;
        Duration = float.MaxValue;
        Type = EnemyTypesEnum.Spider;
    }

    public EnemiesModel(int lives,
                        float speed,
                        int damage,
                        float duration,
                        EnemyTypesEnum type)
    {
        Lives = lives;
        Speed = speed;
        Damage = damage;
        Duration = duration;
        Type = type;
    }
}
