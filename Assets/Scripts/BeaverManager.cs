using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BeaverManager : MonoBehaviour
{
    private static BeaverManager instance;
    public static BeaverManager Instance { get { return GetInstance(); } }
    
    private static BeaverManager GetInstance()
    {
        if(instance == null)
        {
            instance = new BeaverManager();
        }

        return instance;
    }

    private DamGenerator damGenerator;
    private DamGroup theDam;
    private OrderGenerator orderGenerator;

    public DamGroup TheDam { get => theDam; }
    public OrderGenerator OrderGenerator { get => orderGenerator; set => OrderGenerator = value; }

    public List<BeaverData> Beavers { get; private set; }


    [SerializeField]
    BeaverNamer namer;

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
                //Debug.Log("Made Beav");
                AddBeaver();
            }
            namer.nameBeavers(Beavers);
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

    public BeaverData getBeaverFromName(string name)
    {
        for(int i = 0;i < Beavers.Count; i++)
        {
            if (Beavers[i].BeaverName == name)
            {
                return Beavers[i];
            }
        }
        return null;
    }

}
