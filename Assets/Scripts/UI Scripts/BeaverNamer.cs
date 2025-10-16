using NUnit.Framework;
using TMPro;
using UnityEngine;
using System.Collections.Generic;

public class BeaverNamer : MonoBehaviour
{
    [SerializeField]
    List<TextMeshProUGUI> beaverText;

    bool beaversNamed = false;
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
        Debug.Log("Beavers Named");
        for (int i = 0; i < beaverText.Count; i++)
        {
            beaverText[i].text = beavers[i].BeaverName + "\n¥" + beavers[i].Intelligence + " §" + beavers[i].Speed;
        }
    }
}
