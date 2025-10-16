using TMPro;
using UnityEngine;

public class MemoryDisplay : MonoBehaviour
{
    [Tooltip("how many lines of text should be displayed before we cut them off")]
    public int numberOfLinesDisplayed = 10;
    [Tooltip("what to display this text on")]
    public TextMeshProUGUI DisplayTextDialogue;
    private string totalMemories = "";
    private string displayMemories = "";


    /// <summary>
    /// adds the beaver's memories to the total memories, and parses it for display
    /// </summary>
    /// <param name="beaver"></param>
    public void ReturnBeaver(BeaverData beaver)
    {
        totalMemories += beaver.BeaverName + "said: \n";
         totalMemories += beaver.DropOffMemories();

        ParseMemories();
    }

    /// <summary>
    /// converts the entire memory bank into the lines that we can actually see
    /// </summary>
    private void ParseMemories()
    {
        string[] parsedMemories = totalMemories.Split('\n');
        for(int i = 0; i < 10; i++)
        {
            displayMemories += parsedMemories[parsedMemories.Length - i] + '\n';
        }
        DisplayTextDialogue.text = displayMemories;
    }
}