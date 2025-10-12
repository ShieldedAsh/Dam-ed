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
    [Tooltip("a complete filepath to save the data to")]
    public string filePath;
    public static List<string> beaverNames;

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
    private bool LoadBeavers()
    {
        Stream sr = null;
        BinaryReader br = null;

        try
        {
            //if the file does not exist already, create it
            if (!File.Exists(filePath))
            {
                sr = File.Create(filePath);
                BinaryWriter bw = new BinaryWriter(sr);
                bw.Write(0);
                sr.Flush();
                sr.Close();
            }

            sr = File.OpenRead(filePath);
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
            if (!File.Exists(filePath))
            {
                sw = File.Create(filePath);
                bw = new BinaryWriter(sw);
                sw.Close();
            }
            sw = File.OpenWrite(filePath);
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
        return true;
    }
    /// <summary>
    /// saves beavers to the filepath
    /// </summary>
    /// <param name="beavers">beavers to save</param>
    /// <returns>true if read the file succesfully, otherwise false</returns>
    public bool SaveMoreBeavers(List<string> beavers)
    {
        Stream sw = null;
        BinaryWriter bw = null;
        LoadBeavers();
        try
        {
            if (!File.Exists(filePath))
            {
                sw = File.Create(filePath);
                bw = new BinaryWriter(sw);
                sw.Close();
            }
            sw = File.OpenWrite(filePath);
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
        return true;
    }
}