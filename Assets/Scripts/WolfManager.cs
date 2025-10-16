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
        if (wolfCount <= 0)
        {
            wolfCount = 2;
        }
        wolves = new List<Wolf>();
        //should increase how many cells are excluded

        for (int i = 0; i < wolfCount; i++)
        {
            List<DamCell> exclude = new List<DamCell>() { theDam.HQ };
            wolves.Add(new Wolf(this, theDam.GetRandomCell(exclude)));
            exclude.Add(wolves[i].CurrentLocation);
            wolves[i].ChangeTarget(theDam.GetRandomCell(exclude));
        }
    }

    private void Update()
    {
        foreach (Wolf wolf in wolves)
        {
            if (wolf.MainTarget == null)
            {
                wolf.ChangeTarget(theDam.GetRandomCell(new List<DamCell> { TheDam.HQ, wolf.CurrentLocation }));
            }
            wolf.UpdateWolf();
        }
    }
}
