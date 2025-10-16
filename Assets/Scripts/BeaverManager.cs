using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BeaverManager : MonoBehaviour
{
    private DamGenerator damGenerator;
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
        damGenerator = FindAnyObjectByType<DamGenerator>();
        theDam = damGenerator.Dam;
        orderGenerator = FindAnyObjectByType<OrderGenerator>();
    }

    private void Update()
    {
        damGenerator.AttemptToRepairConnections();
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
                if (beaver.Orders[0] != null && beaver.Orders[0].CurrentPath.Count == 1)
                {
                    Debug.Log("Error at: " + beaver.CurrentLocation);
                }

                beaver.UpdateBeaver();
            }
        }
    }

    public void AddBeaver()
    {
        Beavers.Add(new BeaverData(this));
    }

}
