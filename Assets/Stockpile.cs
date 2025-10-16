using TMPro;
using UnityEngine;

public class Stockpile : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI foodStock;
    [SerializeField]
    TextMeshProUGUI scrapStock;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        foodStock.text = HQ.Instance.totalFood.ToString();
        scrapStock.text = HQ.Instance.totalScrap.ToString();
    }
}
