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

    /// <summary>
    /// checks to make sure the item is scrap, and if so, add it to this count
    /// </summary>
    /// <param name="scrap">the scrap to add</param>
    public void AddScrap(IItem scrap)
    {
        if (scrap is Scrap)
        {
            count += ((Scrap)scrap).count;
        }
    }
}
