using UnityEngine;

public class HQ : MonoBehaviour
{
    public BeaverManager Beavers { get { return beavers; } set { beavers = value; } }
    private BeaverManager beavers;

    private int totalScrap;
    private int totalFood;

    public void AddScrap(Scrap scrap)
    {
        totalScrap += 1;
    }

    public void TakeScrap()
    {
        totalScrap -= 1;
    }

    public void AddFood(Food food)
    {
        totalFood += food.Count;
    }
}
