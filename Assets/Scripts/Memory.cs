using System.Collections.Generic;
using UnityEngine;

public class Memory
{

    /// <summary>
    /// The cell the memory is found on
    /// </summary>
    public DamCell Cell { get; private set; }
    /// <summary>
    /// What the Beaver found at that position
    /// </summary>
    public List<IItem> Knowledge { get; private set; }

    /// <summary>
    /// Constructor for a memory
    /// </summary>
    /// <param name="cell">The cell the memory is about</param>
    public Memory(DamCell cell)
    {
        Cell = cell;
        Knowledge = new List<IItem>();
    }

    /// <summary>
    /// Adds an item to the memory
    /// </summary>
    /// <param name="item">The item to add</param>
    public void AddItem(IItem item)
    {
        Knowledge.Add(item);
    }

    /// <summary>
    /// Prints out what a Beaver's memory includes. Needs tuning
    /// </summary>
    /// <returns>A string of the memory</returns>
    public override string ToString()
    {
        if(Knowledge.Count == 0)
        {
            return "I saw nothing";
        }
        string output = "I saw ";
        foreach (IItem mem in Knowledge)
        {
            switch (mem.itemType)
            {
                case IItem.ItemType.Beaver:
                    BeaverData beaver = (BeaverData)mem;
                    output += "our friend " + beaver.BeaverName;
                    if (beaver.BeaverStatus == BeaverData.Status.Dead)
                    {
                        output += " he was dead. I also saw ";
                    }
                    else if (beaver.BeaverStatus == BeaverData.Status.Injured)
                    {
                        output += " he was injured. I also saw ";
                    }
                    else
                    {
                        output += $" he was {beaver.CurrentOrder}. I also saw ";
                    }
                        break;
                case IItem.ItemType.Wolf:
                    output += " a wolf, ";
                    break;
                case IItem.ItemType.Food:
                    output += $"a stack of {((Food)mem).Count} pieces of food, ";
                    break;
                case IItem.ItemType.Scrap:
                    output += $"a stack of {((Scrap)mem).Count} pieces of scrap, ";
                    break;
            }
        }
        return output + "and nothing more.";
    }
}
