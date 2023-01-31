public struct ItemsStruct
{
    public string Name { get; private set; }
    public ItemsTypeEnum Type { get; private set; }
    public int Count { get; private set; }

    public ItemsStruct(string name,
                       ItemsTypeEnum type,
                       int count = 1)
    {
        Name = name;
        Type = type;
        Count = count;
    }
}
