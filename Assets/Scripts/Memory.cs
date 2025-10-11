using System.Collections.Generic;
using UnityEngine;

public class Memory : MonoBehaviour
{
    /// <summary>
    /// Things a beaver can remember
    /// </summary>
    public enum Knowledge
    {
        DeadBeaver,
        InjuredBeaver,
        Food,
        Scrap,
        WasInjured,
        Wolf,
        Blockadge
    }

    /// <summary>
    /// The cell the memory is found on (currently an (int, int), needs to be replaced with actual cell)
    /// </summary>
    public (int, int) cell { get; private set; }
    /// <summary>
    /// What the Beaver found at that position
    /// </summary>
    public List<Knowledge> knowledge { get; private set; }

    /// <summary>
    /// Constructor for a memory (CELL MUST BE UPDATED)
    /// </summary>
    /// <param name="cell">The cell the memory is about</param>
    /// <param name="knowledge"">What the beaver remembers 
    /// (list because thoretically there could be any number of things on a cell)</param>
    public Memory(int cell, List<Knowledge> knowledge)
    {
        this.cell = (cell, cell);
        this.knowledge = knowledge;
    }

    /// <summary>
    /// Prints out what a Beaver's memory includes. Needs tuning
    /// </summary>
    /// <returns>A string of the memory</returns>
    public override string ToString()
    {
        if(knowledge.Count == 0)
        {
            return "I saw nothing";
        }
        string output = "I saw ";
        foreach (Knowledge mem in knowledge)
        {
            switch (mem)
            {
                case Knowledge.DeadBeaver:
                    output += "a dead beaver, ";
                    break;
                case Knowledge.InjuredBeaver:
                    output += "an injured beaver, ";
                    break;
                case Knowledge.Food:
                    output += "a unit of food, ";
                    break;
                case Knowledge.Scrap:
                    output += "a unit of scrap, ";
                    break;
                case Knowledge.WasInjured:
                    output += "and I was injured. I also saw ";
                    break;
                case Knowledge.Wolf:
                    output += "a wolf, ";
                    break;
                case Knowledge.Blockadge:
                    output += "a blockadge, ";
                    break;
            }
        }
        return output + "and nothing more.";
    }
}
