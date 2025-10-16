using System.Collections.Generic;
using UnityEngine;

public class WolfManager : MonoBehaviour
{
    private List<Wolf> wolves;
    private int wolfCount;
    private DamGroup theDam;

    public DamGroup TheDam { get => theDam; }

    public int WolfCount { get => wolfCount; set => wolfCount = value; }

    public List<Wolf> Wolves { get { return wolves; } }

    private void Start()
    {
        theDam = FindAnyObjectByType<DamGenerator>().Dam;
        //should increase how many cells are excluded
        List<DamCell> exclude = new List<DamCell>() { theDam.HQ };

        for (int i = 0; i < wolfCount; i++)
        {
            wolves.Add(new Wolf(this, theDam.GetRandomCell(exclude)));
            wolves[i].ChangeTarget(theDam.GetRandomCell(exclude));
        }
    }

    private void Update()
    {
        foreach (Wolf wolf in wolves)
        {
            if (wolf.MainTarget == null)
            {
                wolf.ChangeTarget(theDam.GetRandomCell(new List<DamCell> { TheDam.HQ }));
            }
            wolf.UpdateWolf();
        }
    }
}
