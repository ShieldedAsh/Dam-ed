using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GenerateBeaverNames : MonoBehaviour
{
    public TMP_InputField[] beaverNames;
    private BeaverManager beaverManager;
    public bool canContinue = false;
    private void Awake()
    {
        beaverManager = FindFirstObjectByType<BeaverManager>();
    }

    public void _AssignBeaverNames()
    {
        List<BeaverData> beavers = beaverManager.Beavers;
        string[] beaverTextNames = new string[9];
        if(beaverNames.Length == 9)
        {
            for(int i = 0; i < beaverNames.Length; i++)
            {
                if (SaveAndValidatebeavers.ValidBeaverName(beaverNames[i].text))
                {
                    beavers[i].BeaverName = beaverNames[i].text;
                    beaverTextNames[i] = beaverNames[i].text;
                }
                else
                    return;
            }
            SaveAndValidatebeavers.SaveMoreBeavers(beaverTextNames);
            canContinue = true;
            this.gameObject.SetActive(false);
        }
    }
}
