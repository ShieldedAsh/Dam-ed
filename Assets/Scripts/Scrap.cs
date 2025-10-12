using UnityEngine;

public class Scrap : Item
{
    //Add code to this later, just exists to connect it for now
    
    /// <summary>
    /// This is a Scrap item
    /// </summary>
    public Item.ItemType itemType { get { return Item.ItemType.Scrap; } }

    /// <summary>
    /// How many scrap items are here
    /// </summary>
    public int Count { get; }
}
