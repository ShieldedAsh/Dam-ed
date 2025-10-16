using UnityEngine;
using TMPro;
using System.Collections.Generic;
using Unity.VisualScripting;


public class OrderMaker : MonoBehaviour
{
    [SerializeField]
    TMP_Dropdown beaver;
    [SerializeField]
    TMP_Dropdown order;
    [SerializeField]
    TMP_InputField coordinates;

    string selectedOrder = "Move to";
    string selectedBeaver;

    public List<Order> orders;

    BeaverData activeBeaver;

    [SerializeField] BeaverManager beaverManager;
    [SerializeField] OrderGenerator orderGenerator;

    Order testOrder;

    string orderString;

    [SerializeField]
    TextMeshProUGUI ordersList;

    [SerializeField] MemoryDisplay memoryDisplay;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        orders = new List<Order>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ChangeOrder()
    {
        selectedOrder = order.captionText.text;
        Debug.Log(selectedOrder);
    }

    public void ChangeBeaver()
    {
        selectedBeaver = beaver.captionText.text;
        Debug.Log(selectedBeaver);
        activeBeaver = beaverManager.getBeaverFromName(selectedBeaver);
        orders.Clear();
        ordersList.text = "";
    }

    public void AddOrder()
    {
        
        switch (selectedOrder)
        {
            case "Move to":
                testOrder = orderGenerator.TryCreateMoveOrder(activeBeaver, coordinates.text);
                Debug.Log(testOrder);
            break;

            case "Scavenge":
                testOrder = orderGenerator.CreateScavengeOrder(activeBeaver);
                Debug.Log(testOrder);
                break;

            case "Distract":
                testOrder = orderGenerator.CreateDistractOrder(activeBeaver);
                Debug.Log(testOrder);
                break;

            case "Barricade to":
                testOrder = orderGenerator.TryCreateBarricadeOrder(activeBeaver, coordinates.text);
                Debug.Log(testOrder);
                break;

            case "Tunnel to":
                testOrder = orderGenerator.TryCreateTunnelOrder(activeBeaver, coordinates.text);
                Debug.Log(testOrder);
                break;
            
            default:
                
                break;

        }
        if(testOrder == null)
        {
            memoryDisplay.passMemory("The beavers do not comprehend.");
        }
        if(testOrder != null && orders.Count < activeBeaver.Intelligence)
        {
            orders.Add(testOrder);
            ordersList.text = ordersList.text + "\n" + selectedOrder.ToString();
        }
        

    }

    public void GiveOrder()
    {
        foreach(var order in orders)
        {
            activeBeaver.GiveOrder(order);
        }
        orders.Clear();
        ordersList.text = "";
    }
}
