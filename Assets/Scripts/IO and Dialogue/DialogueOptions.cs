using System.Collections.Generic;
using System.IO;
using UnityEngine;
using static UnityEditor.Rendering.CameraUI;

public enum dataTypes
{
    Unknown,
    itemType,
    BeaverStatus,
}
public struct extractedData
{
    [Tooltip("what type of enum is this data")]
    public dataTypes data;
    [Tooltip("what should trigger this")]
    public string dataName;
    [Tooltip("phrases that can be said")]
    private string[] phrases;
    [Tooltip("a un-edited phrase for this data")]
    public string GetPhrase { get { return phrases[Random.Range(0, phrases.Length)]; } }

    /// <summary>
    /// parses the raw dialogue data
    /// </summary>
    /// <param name="data">what type of enum is this data</param>
    /// <param name="dataName">what should trigger this</param>
    /// <param name="phrases">what phrases should be able to be said</param>
    public extractedData(dataTypes data, string dataName, string phrases)
    {
        this.data = data;
        this.dataName = dataName;
        this.phrases = phrases.Split('~');
    }
}
public class DialogueOptions : MonoBehaviour
{
    [Tooltip("the dialogue file")]
    public TextAsset filePath;
    public static List<extractedData> reservedPhrases = new List<extractedData>();


    private void Awake()
    {
        Loadfile();
    }
    /// <summary>
    /// finds and parses the dialogue file
    /// </summary>
    /// <returns>true if it successfully reads the file, otherwise false</returns>
    public bool Loadfile()
    {
        /*
        Stream sr = null;
        StreamReader br = null;
        try
        {
            sr = File.OpenRead(filePath);
            br = new StreamReader(sr); 
        }
        catch
        {
            return false;
        }
        string words = br.ReadToEnd();
        br.Close();
        */

        string words = filePath.text;
        char pastChar = ' ';
        bool isComment = false;
        for (int i = 0; i < words.Length; i++)
        {
            char character = words[i];
            if (!isComment)
            {
                switch (character)
                {
                    case '#':
                    case '$':
                        char newCheckerChar = character;
                        int enumIndexRef = i;
                        while (newCheckerChar != ':')
                        {
                            newCheckerChar = words[enumIndexRef];
                            enumIndexRef++;
                        }
                        int chunkIndex = enumIndexRef + 1;
                        while (newCheckerChar != ';')
                        {
                            chunkIndex++;
                            if (words[chunkIndex] == '\\')
                            {
                                chunkIndex += 2;
                            }
                            newCheckerChar = words[chunkIndex];
                        }
                        reservedPhrases.Add(new extractedData(getDataType(words[i..enumIndexRef]), words[++i..--enumIndexRef], words[++enumIndexRef..chunkIndex]));
                        break;
                }
            }
            if(pastChar == '/' && character == '*')
            {
                isComment = true;
            }
            else if(pastChar == '*' && character == '/')
            {
                isComment = false;
            }
            pastChar = character;
        }

        return true;
    }
    /// <summary>
    /// finds what enum in dataType this word is
    /// </summary>
    /// <param name="word">the word to check</param>
    /// <returns>a dataType that references what enum it should be</returns>
    private dataTypes getDataType(string word)
    {
        switch (word[0])
        {
            case '#':
                return dataTypes.itemType;
            case '$':
                return dataTypes.BeaverStatus;
            default:
                return dataTypes.Unknown;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="memory"></param>
    public static string RecallMemory(Memory memory)
    {
        string parsedMemory = "";
        foreach(IItem item in memory.Knowledge)
        {
            foreach(extractedData textData in reservedPhrases)
            {
                if (item.ToString().Equals(textData.dataName))
                {
                    string fixedText = textData.GetPhrase;
                    switch (item)
                    {
                        case BeaverData:
                            fixedText.Replace("\\;", ((BeaverData)item).BeaverName);
                            fixedText.Replace("\\|", ((BeaverData)item).CurrentOrder.ToString());
                            break;
                        case Food:
                            fixedText.Replace("\\+", "" + ((Food)item).Count);
                            break;
                        case Scrap:
                            fixedText.Replace("\\-", "" + ((Scrap)item).Count);
                            break;

                    }
                    parsedMemory += fixedText + " and ";
                }
            }
        }
        return parsedMemory;
    }
}