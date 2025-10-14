using UnityEngine;

public interface IItem
{
    /// <summary>
    /// Types of items that exist
    /// </summary>
    enum ItemType
    {
        Wolf,
        Beaver,
        Food,
        Scrap
    }

    /// <summary>
    /// Gets the type of the item
    /// </summary>
    ItemType itemType { get; }
}
