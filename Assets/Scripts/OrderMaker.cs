using UnityEngine;
using TMPro;
using System.Collections.Generic;


public class OrderMaker : MonoBehaviour
{
    [SerializeField]
    TMP_Dropdown beaver;
    [SerializeField]
    TMP_Dropdown order;
    [SerializeField]
    TMP_InputField coordinates;

    string selectedOrder;
    string selectedBeaver;

    public List<Order> orders;

    BeaverData activeBeaver;

    [SerializeField] BeaverManager beaverManager;
    [SerializeField] OrderGenerator orderGenerator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ChangeOrder()
    {
        selectedOrder = order.captionText.text;
        //Debug.Log(selectedOrder);
    }

    public void ChangeBeaver()
    {
        selectedBeaver = beaver.captionText.text;
        Debug.Log(selectedBeaver);
        activeBeaver = beaverManager.getBeaverFromName(selectedBeaver);
        
    }

    public void AddOrder()
    {
        
        switch (selectedOrder)
        {
            case "Move to":
                
            break;

            case "Scavenge":
                break;

            case "Distract":
                break;

            case "Barricade to":
                break;

            case "Tunnel to":
                break;
            
            default:
                
                break;

        }
    }

    public void GiveOrder()
    {

    }
}
