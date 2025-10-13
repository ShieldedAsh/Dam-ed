using System.Collections.Generic;
using System.IO;
using UnityEngine;

enum dataTypes
{

    ItemType,
    BeaverStatus,
}
struct extractedData
{
    public dataTypes data;
    public string dataName;
    public string phrase;
    public extractedData(dataTypes data, string dataName, string phrase)
    {
        this.data = data;
        this.dataName = dataName;
        this.phrase = phrase;
    }
}
public class DialogueOptions : MonoBehaviour
{
    public string filePath;

    private void Start()
    {
        Loadfile();
    }
    public bool Loadfile()
    {
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

        char pastChar = ' ';
        bool isComment = false;
        List<extractedData> reservedPhrases = new List<extractedData>();
        for (int i = 0; i < words.Length; i++)
        {
            char character = words[i];
            if (!isComment)
            {
                switch (character)
                {
                    case '#':
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

                        reservedPhrases.Add(new extractedData(getDataType(words[++i..--enumIndexRef]), words[++i..--enumIndexRef], words[++enumIndexRef..chunkIndex]));
                        break;
                    case '$':
                        break;
                }
            }
            switch (pastChar)
            {
                case '\\':
                    if (!isComment)
                    {
                        switch (character)
                        {
                            case ';':
                                break;
                            case '|':
                                break;
                            case '+':
                                break;
                            case '-':
                                break;
                        }
                    }
                    break;
                case '*':
                    if (character == '/')
                    {
                        isComment = false;
                    }
                    break;
                case '/':
                    if(character == '*')
                    {
                        isComment = true;
                    }
                    break;
            }
            pastChar = character;
        }

        return true;
    }

    private dataTypes getDataType(string word)
    {
        if (word[0] == '#')
        {
            return dataTypes.ItemType;
        }
        else
        {
            return default;
        }
    }
}