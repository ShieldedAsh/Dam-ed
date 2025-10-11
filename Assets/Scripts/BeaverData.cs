using UnityEngine;
using System.Collections.Generic;

public class BeaverData : MonoBehaviour
{
    /// <summary>
    /// Items a beaver can carry
    /// </summary>
    public enum Carrying
    {
        Scrap,
        Food,
        Beaver,
        Nothing
    }

    /// <summary>
    /// The name of the Beaver
    /// </summary>
    public string beaverName { get; private set; }
    /// <summary>
    /// The intelligence of the Beaver
    /// </summary>
    public int intelligence { get; private set; }
    /// <summary>
    /// The speed of the beaver
    /// </summary>
    public int speed {  get; private set; }
    /// <summary>
    /// The memories of the Beaver
    /// </summary>
    public List<Memory> memory { get; private set; }
    /// <summary>
    /// The beaver's current orders (NOTE: Length = intelligence + 1, as last order is always a move order to HQ)
    /// </summary>
    public Order[] orders { get; private set; }
    private int currentOrderIndex;
    /// <summary>
    /// The order the beaver is currently carrying out
    /// </summary>
    public Order currentOrder { get { return orders[currentOrderIndex]; } }
    /// <summary>
    /// What the beaver is actively carrying;
    /// </summary>
    public Carrying carrying { get; private set; }
    /// <summary>
    /// Is the beaver dead
    /// </summary>
    public bool isDead { get; private set; }

    /// <summary>
    /// Creates a beavers with a given name as well as intelligence and speed stats
    /// </summary>
    /// <param name="name">The Beaver's name</param>
    /// <param name="intelligence">The Beaver's intelligence</param>
    /// <param name="speed">The Beaver's speed</param>
    public BeaverData(string name, int intelligence, int speed)
    {
        beaverName = name;
        this.intelligence = intelligence;
        this.speed = speed;
        memory = new List<Memory>();
        orders = new Order[intelligence + 1];
        carrying = Carrying.Nothing;
        isDead = false;
    }

    /// <summary>
    /// Default constructor for a beaver. sets the name to BGDD-R(2) and the intelligence and speed to 5
    /// </summary>
    public BeaverData()
    {
        beaverName = "BGDD-R(2)";
        intelligence = 5;
        speed = 5;
        memory = new List<Memory>();
        orders = new Order[intelligence + 1];
        orders[orders.Length] = new Order(Order.Action.Move);
        carrying = Carrying.Nothing;
        isDead = false;
    }

    /// <summary>
    /// UPDATE TO ACTUALLY TAKE A ROOM THING!!!!!
    /// </summary>
    public void EvaluateRoom()
    {

    }

    /// <summary>
    /// Trys to add an order to the Beaver's list
    /// </summary>
    /// <param name="order">The order</param>
    /// <returns>Whether or not the order was added</returns>
    public bool GiveOrder(Order order)
    {
        for(int i = 0; i < orders.Length - 1; i++)
        {
            if (orders[i] == null)
            {
                orders[i] = order;
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
        currentOrder.TakeAction();
        currentOrderIndex++;
    }
}
