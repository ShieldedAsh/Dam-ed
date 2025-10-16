using UnityEngine;
using UnityEngine.Windows;

public class OrderGenerator : MonoBehaviour
{
    #nullable enable
    private BeaverManager beaverManager;

    private void Start()
    {
        beaverManager = FindFirstObjectByType<BeaverManager>();
    }

    /// <summary>
    /// Attempts to create a move order
    /// </summary>
    /// <param name="beaver">The beaver attempting to move</param>
    /// <param name="target">the inputed location</param>
    /// <returns>The created order, if the location was invalid it returns null</returns>
    public Order? TryCreateMoveOrder(BeaverData beaver, string target)
    {
        target = target.ToLower();
        if (IsValidCell(target))
        {
            return new Order(Order.Action.Move, beaver, beaverManager, beaverManager.TheDam.Cells[target[0] - 97, int.Parse(target.Substring(1, target.Length - 1)) - 1]);
        }
        return null;
    }

    /// <summary>
    /// Creates a scavenge order
    /// </summary>
    /// <param name="beaver">The beaver scavenging</param>
    /// <returns>The created order</returns>
    public Order CreateScavengeOrder(BeaverData beaver)
    {
        return new Order(Order.Action.Scavenge, beaver, beaverManager);
    }

    /// <summary>
    /// Attempts to create a barricade order
    /// </summary>
    /// <param name="beaver">The beaver attempting to barricade</param>
    /// <param name="target">the inputted location</param>
    /// <returns>The created order, if the location was invalid it returns null</returns>
    public Order? TryCreateBarricadeOrder(BeaverData beaver, string target)
    {
        target = target.ToLower();
        if (IsValidCell(target))
        {
            return new Order(Order.Action.Barricade, beaver, beaverManager, beaverManager.TheDam.Cells[target[0] - 97, int.Parse(target.Substring(1, target.Length - 1)) - 1]);
        }
        return null;
    }

    /// <summary>
    /// Attempts to create a tunnel order
    /// </summary>
    /// <param name="beaver">The beaver attempting to tunnel</param>
    /// <param name="target">the inputted location</param>
    /// <returns>The created order, if the location was invalid it returns null</returns>
    public Order? TryCreateTunnelOrder(BeaverData beaver, string target)
    {
        target = target.ToLower();
        if (IsValidCell(target))
        {
            return new Order(Order.Action.Tunnel, beaver, beaverManager, beaverManager.TheDam.Cells[target[0] - 97, int.Parse(target.Substring(1, target.Length - 1)) - 1]);
        }
        return null;
    }

    /// <summary>
    /// Creates a distraction order
    /// </summary>
    /// <param name="beaver">The beaver distracting</param>
    /// <returns>The created order</returns>
    public Order CreateDistractOrder(BeaverData beaver)
    {
        return new Order(Order.Action.Distract, beaver, beaverManager);
    }

    /// <summary>
    /// checks to see if the inputted cell is within bounds
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public bool IsValidCell(string input)
    {
        Debug.Log("called");
        if (beaverManager.TheDam.Cells.GetLength(1) < 10)
        {
            Debug.Log("small grid");
            if (input.Length != 2)
            {
                Debug.Log("Not 2");
                return false;
            }
            if ((int)input[0] < 97 || (int)input[0] >= 97 + beaverManager.TheDam.Cells.GetLength(0))
            {
                Debug.Log("character out of range");
                return false;
            }
            if (input[1] < 1 ) 
            {
                Debug.Log("less than 1");
                return false;
            }
            Debug.Log(input[1]);
            Debug.Log(beaverManager.TheDam.Cells.GetLength(1));
            if (int.Parse(input.Substring(1, input.Length - 1)) >= beaverManager.TheDam.Cells.GetLength(1))
            {
                Debug.Log("greater than length");
                return false;
            }
            return true;
        }
        else 
        {
            Debug.Log("Large grid");
            if (input.Length < 2)
            {
                return false;
            }
            if ((int)input[0] < 97 || (int)input[0] >= 97 + beaverManager.TheDam.Cells.GetLength(0))
            {
                Debug.Log("character out of range");
                return false;
            }

            int column;
            if (!int.TryParse(input.Substring(1, input.Length - 1), out column))
            {
                Debug.Log("Could not parse");
                return false;
            }
            else
            {
                if (column < 1 || column >=  beaverManager.TheDam.Cells.GetLength(1))
                {
                    Debug.Log("out of range");
                    return false;
                }
            }
            return true;
        }
    }
}
