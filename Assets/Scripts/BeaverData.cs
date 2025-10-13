using UnityEngine;
using System.Collections.Generic;

[System.Serializable]

public class BeaverData : IItem
{
    #nullable enable
    /// <summary>
    /// The name of the Beaver
    /// </summary>
    public string BeaverName { get; private set; }

    /// <summary>
    /// The intelligence of the Beaver
    /// </summary>
    public int Intelligence { get; private set; }

    /// <summary>
    /// The speed of the beaver
    /// </summary>
    public float Speed {  get; private set; }

    /// <summary>
    /// The memories of the Beaver
    /// </summary>
    public List<Memory> Memory { get; private set; }

    /// <summary>
    /// The beaver's current orders (NOTE: Length = intelligence + 1, as last order is always a move order to HQ)
    /// </summary>
    public Order[] Orders { get; private set; }

    /// <summary>
    /// The order the beaver is currently carrying out
    /// </summary>
    public Order? CurrentOrder { get { return Orders[currentOrderIndex]; } }
    private int currentOrderIndex;

    /// <summary>
    /// What the beaver is actively carrying;
    /// </summary>
    public IItem Carrying { get; set; }

    /// <summary>
    /// Enum for the status of the Beaver
    /// </summary>
    public enum Status
    {
        Dead,
        Injured,
        Healthy
    }

    /// <summary>
    /// What is the beaver's current status
    /// </summary>
    public Status BeaverStatus { get; private set; }

    /// <summary>
    /// The current location of the beaver
    /// </summary>
    public DamCell CurrentLocation { get; set; }
    
    /// <summary>
    /// This is a Beaver item
    /// </summary>
    public IItem.ItemType itemType { get { return IItem.ItemType.Beaver; } }

    private float timeToMove = 0;

    private BeaverManager beaverManager;

    public void UpdateBeaver()
    {
        if(CurrentOrder != null)
        {
            if (timeToMove <= 0)
            {
                timeToMove = Random.Range(2f - Speed, 3f - Speed);
                ExecuteOrder();
            }
            timeToMove -= Time.deltaTime;
        }
    }

    /// <summary>
    /// Creates a beavers with a given name as well as intelligence and speed stats
    /// </summary>
    /// <param name="name">The Beaver's name</param>
    /// <param name="intelligence">The Beaver's intelligence</param>
    /// <param name="speed">The Beaver's speed</param>
    public BeaverData(BeaverManager beaverManager, string name, int intelligence, float speed) : this(beaverManager)
    {
        BeaverName = name;
        Intelligence = intelligence;
        Speed = speed;
        Orders = new Order[Intelligence + 1];
        //Orders[Orders.Length - 1] = new Order(Order.Action.Move, this, beaverManager, beaverManager.TheDam.HQ);
    }

    /// <summary>
    /// Default constructor for a beaver. sets the name to BGDD-R(2) and the intelligence to 5 and speed to 1
    /// </summary>
    public BeaverData(BeaverManager beaverManager)
    {
        BeaverName = "BGDD-R(2)";
        Intelligence = 5;
        Speed = 1;
        Memory = new List<Memory>();
        Orders = new Order[Intelligence + 1];
        //Orders[Orders.Length - 1] = new Order(Order.Action.Move, this, beaverManager, beaverManager.TheDam.HQ);
        Carrying = default!;
        BeaverStatus = Status.Healthy;
        CurrentLocation = beaverManager.TheDam.HQ;
    }

    /// <summary>
    /// Evaluates a room and adds everything in it to the Beaver's memory (can later be used to check if a beaver survives an encounter with a wolf)
    /// </summary>
    public void EvaluateRoom()
    {
        Memory.Add(new Memory(CurrentLocation));
        foreach(IItem item in CurrentLocation.Contents)
        {
            Memory[Memory.Count - 1].AddItem(item);
        }
    }

    /// <summary>
    /// Trys to add an order to the Beaver's list
    /// </summary>
    /// <param name="order">The order</param>
    /// <returns>Whether or not the order was added</returns>
    public bool GiveOrder(Order order)
    {
        for(int i = 0; i < Orders.Length - 1; i++)
        {
            if (Orders[i] == null)
            {
                Orders[i] = order;
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Executes the next order in the order list
    /// </summary>
    public void ExecuteOrder()
    {
        CurrentOrder!.TakeAction();
        if(CurrentOrder.ThisOrder == Order.Action.Move)
        {
            if(CurrentLocation == CurrentOrder.TargetDamCell)
            {
                currentOrderIndex++;
            }
        }
        else
        {
            currentOrderIndex++;
        }
    }
}
