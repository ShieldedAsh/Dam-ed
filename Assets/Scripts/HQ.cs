using UnityEngine;

public class HQ : MonoBehaviour
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

    private int totalScrap;
    private int totalFood;

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

    public void TakeScrap()
    {
        totalScrap -= 1;
    }
}
