using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BeaverManager : MonoBehaviour
{
    private DamGroup theDam;
    private OrderGenerator orderGenerator;

    public DamGroup TheDam { get => theDam; }
    public OrderGenerator OrderGenerator { get => orderGenerator; set => OrderGenerator = value; }

    public List<BeaverData> Beavers { get; private set; }

    public void Awake()
    {
        Beavers = new List<BeaverData>();
    }
    public void Start()
    {
        theDam = FindAnyObjectByType<DamGenerator>().Dam;
        orderGenerator = FindAnyObjectByType<OrderGenerator>();
    }

    private void Update()
    {
        if (Beavers != null && Beavers.Count == 0 && DamGenerator.hasGenerated)
        {
            for (int i = 0; i < 9; i++)
            {
                AddBeaver();
            }
        }
        
        if (Beavers.Count > 0)
        {
            foreach (BeaverData beaver in Beavers)
            {
                beaver.UpdateBeaver();
            }
        }
    }

    public void AddBeaver()
    {
        Beavers.Add(new BeaverData(this));
    }

}
