using TMPro;
using UnityEngine;

public class MemoryDisplay : MonoBehaviour
{
    [Tooltip("how many lines of text should be displayed before we cut them off")]
    public int numberOfLinesDisplayed = 10;
    [Tooltip("what to display this text on")]
    public TextMeshProUGUI DisplayTextDialogue;
    private string totalMemories = "";
    private string displayMemories;

    private int timesClicked = 0;
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
        displayMemories = "";
        string[] parsedMemories = totalMemories.Split('\n');
        for (int i = 0; i < (parsedMemories.Length < numberOfLinesDisplayed ? parsedMemories.Length : numberOfLinesDisplayed); i++)
        {
            displayMemories += parsedMemories[parsedMemories.Length - 1 - i] + '\n';
        }
        DisplayTextDialogue.text = displayMemories;
    }

    public void OnClick()
    {
        totalMemories += "You clicked a button " + timesClicked++ + " times!" + '\n';
        ParseMemories();
    }

    public void passMemory(string memory)
    {
        totalMemories += memory + '\n';
        ParseMemories();
    }
}