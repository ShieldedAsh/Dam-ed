using NUnit.Framework;
using TMPro;
using UnityEngine;
using System.Collections.Generic;

public class BeaverNamer : MonoBehaviour
{
    [SerializeField]
    List<TextMeshProUGUI> beaverText;

    [SerializeField]
    TMP_Dropdown dropdown;

    List<string> names;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void nameBeavers(List<BeaverData> beavers)
    {
        dropdown.ClearOptions();
        for (int i = 0; i < beaverText.Count; i++)
        {
            beaverText[i].text = beavers[i].BeaverName + "\n¥" + beavers[i].Intelligence + " §" + beavers[i].Speed;
            Debug.Log("Named Beav");

            dropdown.options.Add(new TMP_Dropdown.OptionData(beavers[i].BeaverName));
        }


    }
}
