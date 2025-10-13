using UnityEngine;

public class Scrap : IItem
{
    //Add code to this later, just exists to connect it for now
    private int count;

    /// <summary>
    /// This is a Scrap item
    /// </summary>
    public IItem.ItemType itemType { get { return IItem.ItemType.Scrap; } }

    /// <summary>
    /// How many scrap items are here
    /// </summary>
    public int Count { get => count; }

    public Scrap()
    {
        count = Random.Range(1, 4);
    }
}
