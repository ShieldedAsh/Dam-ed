using UnityEngine;

public class Order : MonoBehaviour
{
    /// <summary>
    /// Things a beaver can do
    /// </summary>
    public enum Action
    {
        Move,
        Scavenge,
        Barricade,
        Tunnel,
        Distract
    }

    /// <summary>
    /// The action contained in this order
    /// </summary>
    public Action order {  get; private set; }

    /// <summary>
    /// Constructor for a single Order
    /// </summary>
    /// <param name="action">The action you want this order to contain</param>
    public Order(Action action)
    {
        order = action;
    }

    //will need a second constructor for orders where the order has a target (move, barricade, tunnel), but need cells first

    /// <summary>
    /// Makes the beaver actually follow through with the order (may want to move to BeaverData? Need to see how things come together)
    /// </summary>
    public void TakeAction()
    {
        switch (order)
        {
            case Action.Move:
                break;
            case Action.Scavenge: 
                break;
            case Action.Barricade: 
                break;
            case Action.Tunnel:
                break;
            case Action.Distract:
                break;
        }
    }
}
