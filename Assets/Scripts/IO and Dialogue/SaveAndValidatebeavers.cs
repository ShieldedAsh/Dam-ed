using System.IO;
using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// file format:
/// int num of names
/// for(num of names)
/// string beaver name
/// </summary>
public class SaveAndValidatebeavers : MonoBehaviour
{
    [Tooltip("the name of the file to save to")]
    [SerializeField] private static string fileName = "beavers.DEAD";
    public static List<string> beaverNames;

    /// <summary>
    /// the actual path for saving the file
    /// </summary>
    private static string actualFilePath { get { return Application.persistentDataPath + "/" + fileName; } }
    /// <summary>
    /// validates that a beaver name has not already been chosen
    /// </summary>
    /// <param name="name">the name to check</param>
    /// <param name="forceOverride">makes this return true even if the name exists</param>
    /// <returns>true if the beaver name can be chosen, otherwise false</returns>
    public static bool ValidBeaverName(string name, bool forceOverride = false)
    {
        if (forceOverride)
        {
            return true;
        }
        else
        {
            name = name.Trim().ToLower();
            for(int i = 0; i < beaverNames.Count; i++)
            {
                if (beaverNames[i].Equals(name))
                {
                    return false;
                }
            }
        }
        return true;
    }

    /// <summary>
    /// loads the beavers from the filepath
    /// </summary>
    /// <returns>true if read the file succesfully, otherwise false</returns>
    private static bool LoadBeavers()
    {
        Stream sr = null;
        BinaryReader br = null;

        try
        {
            //if the file does not exist already, create it
            if (!File.Exists(actualFilePath))
            {
                sr = File.Create(actualFilePath);
                BinaryWriter bw = new BinaryWriter(sr);
                bw.Write(0);
                sr.Flush();
                sr.Close();
            }

            sr = File.OpenRead(actualFilePath);
            br = new BinaryReader(sr);
        }
        catch
        {
            return false;
        }

        int numOfNames = br.ReadInt32();
        beaverNames = new List<string>();
        for(int i = 0; i < numOfNames; i++)
        {
            beaverNames.Add(br.ReadString());
        }
        return true;
    }

    /// <summary>
    /// saves beavers to the filepath
    /// </summary>
    /// <param name="beavers">beavers to save</param>
    /// <returns>true if read the file succesfully, otherwise false</returns>
    public bool SaveMoreBeavers(string[] beavers)
    {
        Stream sw = null;
        BinaryWriter bw = null;
        LoadBeavers();
        try
        {
            if (!File.Exists(actualFilePath))
            {
                sw = File.Create(actualFilePath);
                bw = new BinaryWriter(sw);
                sw.Close();
            }
            sw = File.OpenWrite(actualFilePath);
            bw = new BinaryWriter(sw);
        }
        catch
        {
            return false;
        }

        if (beaverNames != null)
        {
            beaverNames = new List<string>();
        }

        for(int i = 0; i < beavers.Length; i++)
        {
            beaverNames.Add(beavers[i].Trim().ToLower());
        }
        bw.Write(beaverNames.Count);
        for(int i = 0; i < beaverNames.Count; i++)
        {
            bw.Write(beaverNames[i]);
        }
        bw.Flush();
        bw.Close();
        return true;
    }
    /// <summary>
    /// saves beavers to the filepath
    /// </summary>
    /// <param name="beavers">beavers to save</param>
    /// <returns>true if read the file succesfully, otherwise false</returns>
    public static bool SaveMoreBeavers(List<string> beavers)
    {
        Stream sw = null;
        BinaryWriter bw = null;
        LoadBeavers();
        try
        {
            if (!File.Exists(actualFilePath))
            {
                sw = File.Create(actualFilePath);
                bw = new BinaryWriter(sw);
                sw.Close();
            }
            sw = File.OpenWrite(actualFilePath);
            bw = new BinaryWriter(sw);
        }
        catch
        {
            return false;
        }

        if (beaverNames != null)
        {
            beaverNames = new List<string>();
        }

        for (int i = 0; i < beavers.Count; i++)
        {
            beaverNames.Add(beavers[i].Trim().ToLower());
        }
        bw.Write(beaverNames.Count);
        for (int i = 0; i < beaverNames.Count; i++)
        {
            bw.Write(beaverNames[i]);
        }
        bw.Flush();
        bw.Close();
        return true;
    }
}