using TMPro;
using UnityEngine;

public class HQ
{
    private static HQ instance;
    public static HQ Instance { get { return GetInstance(); } }

    private static HQ GetInstance()
    {
        if(instance == null)
        {
            instance = new HQ();
        }

        return instance;
    }

    public BeaverManager Beavers { get { return beavers; } set { beavers = value; } }
    private BeaverManager beavers;

    public DamCell HQCell { get; private set; }
    public DamCell HQLeftCell { get; private set; }
    public int LeftDoorHealth { get; set; }
    public DamCell HQRightCell { get; private set; }
    public int RightDoorHealth { get; set; }

    

    /// <summary>
    /// Constructor for the HQ
    /// </summary>
    public HQ()
    {
        HQCell = null;
        HQLeftCell = null;
        LeftDoorHealth = 10;
        HQRightCell = null;
        RightDoorHealth = 10;
    }

    /// <summary>
    /// Sets the HQ Cell
    /// </summary>
    /// <param name="cell">The cell to use as the HQs</param>
    public void SetHQCell(DamCell cell)
    {
        HQCell = cell;
        foreach(DamCell connected in HQCell.Connections)
        {
            if(connected.CellArrayPosition.X == HQCell.CellArrayPosition.X - 1)
            {
                HQLeftCell = connected;
            }
            else
            {
                HQRightCell = connected;
            }
        }
    }

    public int totalScrap;
    public int totalFood;

    /// <summary>
    /// Adds an item to your inventory
    /// </summary>
    /// <param name="item"></param>
    public void AddItem(IItem item)
    {
        if(item.itemType == IItem.ItemType.Food)
        {
            totalFood += ((Food)item).Count;
        }
        else if(item.itemType == IItem.ItemType.Scrap)
        {
            totalScrap += 1;
        }
    }

    /// <summary>
    /// Lets a beaver take a piece of scrap from the HQ
    /// </summary>
    public void TakeScrap()
    {
        totalScrap -= 1;
    }

    public void EatFood()
    {
        int count = 0;
        foreach (BeaverData beaver in Beavers.Beavers)
        {
            if (beaver.CurrentLocation == HQCell)
            {
                count++;
            }
        }
        totalFood -= 1 + (int)Mathf.Round(count / 2.0f);
    }
    
    //TODO: Add code for beavers eating food
    //TODO: Tie totalScrap and totalFood to their respective variables
}
