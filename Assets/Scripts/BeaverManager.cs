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

    
    public MemoryDisplay memory;

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
            AddBeaver();
            /*
            for (int i = 0; i < 9; i++)
            {
                //Debug.Log("Made Beav");
                AddBeaver();
            }
            */
            namer.nameBeavers(Beavers);
        }
        
        if (Beavers.Count > 0)
        {
            foreach (BeaverData beaver in Beavers)
            {
                if (beaver.Orders[0] != null && beaver.Orders[0].CurrentPath.Count == 1)
                {
                    UnityEngine.Debug.Log("Error at: " + beaver.CurrentLocation);
                }
                /*
                if (beaver.BeaverStatus == BeaverData.Status.Dead)
                {
                    Debug.Log(beaver.BeaverName + " status is: " + beaver.BeaverStatus + " at: " + beaver.CurrentLocation );
                }
                else
                {
                    Debug.Log(beaver.BeaverName + " status is: " + beaver.BeaverStatus);
                }*/
                beaver.UpdateBeaver();
            }
        }
    }

    public void AddBeaver()
    {
        Beavers.Add(new BeaverData(this, "Dave", 5, 5));
        Beavers.Add(new BeaverData(this, "Charlie", 6, 4));
        Beavers.Add(new BeaverData(this, "Austin", 8, 2));
        Beavers.Add(new BeaverData(this, "Josh", 2, 8));
        Beavers.Add(new BeaverData(this, "Mary", 1, 9));
        Beavers.Add(new BeaverData(this, "Kate", 9, 1));
        Beavers.Add(new BeaverData(this, "Autumn", 4, 6));
        Beavers.Add(new BeaverData(this, "Tim", 7, 3));
        Beavers.Add(new BeaverData(this, "Zoe", 3, 7));
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
