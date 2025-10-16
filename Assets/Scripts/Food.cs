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

    /// <summary>
    /// checks to make sure the item is food, and if so, add it to this count
    /// </summary>
    /// <param name="food">the food to add</param>
    public void AddFood(IItem food)
    {
        if(food is Food)
        {
            count += ((Food)food).count;
        }
    }
}
