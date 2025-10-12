using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BeaverManager : MonoBehaviour
{
    private DamGroup theDam;

    public DamGroup TheDam { get => theDam; }

    public List<BeaverData> Beavers { get; private set; }

    public void Awake()
    {
        theDam = FindAnyObjectByType<DamGenerator>().Dam;
    }

    private void Update()
    {
        foreach(BeaverData beaver in Beavers)
        {
            beaver.UpdateBeaver();
        }
    }

}
