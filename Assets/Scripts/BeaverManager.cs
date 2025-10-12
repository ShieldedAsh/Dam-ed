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
        for (int i = 0; i < 9; i++)
        {
            AddBeaver();
        }
        Beavers[0].GiveOrder(new Order(Order.Action.Move, Beavers[0], this, theDam.Cells[7, 8]));
    }

    private void Update()
    {
        foreach(BeaverData beaver in Beavers)
        {
            beaver.UpdateBeaver();
        }
    }

    public void AddBeaver()
    {
        Beavers.Add(new BeaverData(this));
    }

}
