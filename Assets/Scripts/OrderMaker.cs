using UnityEngine;
using TMPro;

public class OrderMaker : MonoBehaviour
{
    [SerializeField]
    TMP_Dropdown beaver;
    [SerializeField]
    TMP_Dropdown order;
    [SerializeField]
    TMP_InputField coordinates;

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void AddOrder()
    {
        TMP_Text currentOrder = order.captionText;
        switch (currentOrder)
        {

        }
    }

    void GiveOrder()
    {

    }
}
