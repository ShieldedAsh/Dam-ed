using UnityEngine;

public class Food : IItem
{
    //Add code to this later, just exists to connect it for now
    private int count;

    /// <summary>
    /// This is a food item
    /// </summary>
    public IItem.ItemType itemType { get { return IItem.ItemType.Food; } }

    /// <summary>
    /// How many Food items are here
    /// </summary>
    public int Count { get => count; }

    public Food()
    {
        count = Random.Range(10, 31);
    }
}
