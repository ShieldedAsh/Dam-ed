using UnityEngine;

public class Scrap : IItem
{
    /// <summary>
    /// This is a Scrap item
    /// </summary>
    public IItem.ItemType itemType { get { return IItem.ItemType.Scrap; } }

    /// <summary>
    /// Scrap constructor (doesn't actually do anything, just makes the item exist)
    /// </summary>
    public Scrap()
    {
        
    }
}
