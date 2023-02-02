using System.Collections.Generic;

public class GoodsModel
{
    public int IndexItem { get; set; }
    public string Name { get; set; }
    public ItemsTypeEnum type { get; set; }
    public List<ItemsModel> Materials { get; set; }

    public GoodsModel(int indexItem,
                      string name,
                      ItemsTypeEnum type,
                      List<ItemsModel> materials)
    {
        IndexItem = indexItem;
        Name = name;
        type = type;
        Materials = materials;
    }
}
