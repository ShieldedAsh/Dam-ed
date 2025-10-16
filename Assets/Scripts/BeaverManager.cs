using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BeaverManager : MonoBehaviour
{
    private DamGroup theDam;

    public DamGroup TheDam { get => theDam; }

    public List<BeaverData> Beavers { get; private set; }


    [SerializeField]
    BeaverNamer namer;

    public void Awake()
    {
        Beavers = new List<BeaverData>();
    }
    public void Start()
    {
        theDam = FindAnyObjectByType<DamGenerator>().Dam;
    }

    private void Update()
    {
        //Debug.Log("Dam: " + DamGenerator.hasGenerated);
        //Debug.Log("Beavers: " + Beavers);

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
