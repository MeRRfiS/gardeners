public struct ItemsStruct
{
    public int Index { get; private set; }
    public string Name { get; private set; }
    public ItemsTypeEnum Type { get; private set; }
    public int Count { get; set; }

    public ItemsStruct(int index,
                       string name,
                       ItemsTypeEnum type,
                       int count = 1)
    {
        Index = index;
        Name = name;
        Type = type;
        Count = count;
    }
}
