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
