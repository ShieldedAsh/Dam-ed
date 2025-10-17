using System;
using UnityEngine;

public class HowToPlay : MonoBehaviour
{

    public Boolean panelActive = false;
    [SerializeField] GameObject panel;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void onClick()
    {
        if (panelActive)
        {
            panelActive = false;
            panel.SetActive(false);
        }
        else
        {
            panelActive = true;
            panel.SetActive(true);
        }
    }
}
