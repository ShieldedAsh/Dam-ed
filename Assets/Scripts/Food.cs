using System;
using UnityEngine;

public class Food : Item
{
    //Add code to this later, just exists to connect it for now
    
    /// <summary>
    /// This is a food item
    /// </summary>
    public Item.ItemType itemType { get { return Item.ItemType.Food; } }

    /// <summary>
    /// How many Food items are here
    /// </summary>
    public int Count { get; }
}
