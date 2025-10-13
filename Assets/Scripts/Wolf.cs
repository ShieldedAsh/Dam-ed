using UnityEngine;

public class Wolf : IItem
{
    //Add code to this later, just exists to connect it for now

    /// <summary>
    /// This is a Wolf item
    /// </summary>
    public IItem.ItemType itemType { get { return IItem.ItemType.Wolf; } }
}
