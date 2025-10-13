using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Order
{
    private List<DamCell> PathToTarget;
    private int currentPathIndex;

    /// <summary>
    /// Things a beaver can do
    /// </summary>
    [System.Serializable]
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
    public Action ThisOrder {  get; private set; }

    /// <summary>
    /// The DamCell being targeted by the order (only exists if the order needs a target)
    /// </summary>
    public DamCell TargetDamCell { get; private set; }

    /// <summary>
    /// The beaver this order is assigned to
    /// </summary>
    private BeaverData beaver;

    [SerializeField] private BeaverManager beaverManager;
    [SerializeField] private Action action;

    /// <summary>
    /// Constructor for a single Order with no target
    /// </summary>
    /// <param name="action">The action you want this order to contain</param>
    /// <param name="beaver">The beaver being given the order</param>
    public Order(Action action, BeaverData beaver, BeaverManager beaverManager)
    {
        ThisOrder = action;
        this.beaver = beaver;
        this.beaverManager = beaverManager;
    }

    /// <summary>
    /// Constructor for a single order with a target
    /// </summary>
    /// <param name="action">The action you want this order to contain</param>
    /// <param name="beaver">The beaver being given the order</param>
    /// <param name="target">The target of the order being taken</param>
    public Order(Action action, BeaverData beaver, BeaverManager beaverManager, DamCell target) : this(action, beaver, beaverManager)
    {   
        TargetDamCell = beaverManager.TheDam.HQ;
        if (ThisOrder == Action.Move)
        {
            PathToTarget = beaverManager.TheDam.GetShortestPath(beaver.CurrentLocation, TargetDamCell);
            currentPathIndex = 0;
        }
    }

    /// <summary>
    /// Makes the beaver actually follow through with the order (may want to move to BeaverData? Need to see how things come together)
    /// </summary>
    public void TakeAction()
    {
        switch (ThisOrder)
        {
            case Action.Move:
                beaver.EvaluateRoom();
                //Re-evaluates Path to find current best route
                PathToTarget = beaverManager.TheDam.GetShortestPath(beaver.CurrentLocation, TargetDamCell);
                currentPathIndex = 0;
                beaver.CurrentLocation = PathToTarget[currentPathIndex];
                currentPathIndex++;
                
                break;
            case Action.Scavenge:
                if(beaver.CurrentLocation.Contents.Count != 0)
                {
                    beaver.Carrying = beaver.CurrentLocation.Contents[beaver.CurrentLocation.Contents.Count - 1];
                    beaver.CurrentLocation.Contents.RemoveAt(beaver.CurrentLocation.Contents.Count - 1);
                }
                break;
            case Action.Barricade:
                beaverManager.TheDam.Cells[beaver.CurrentLocation.CellArrayPosition.X, beaver.CurrentLocation.CellArrayPosition.Y].RemoveConnection(TargetDamCell);
                beaverManager.TheDam.Cells[TargetDamCell.CellArrayPosition.X, TargetDamCell.CellArrayPosition.Y].RemoveConnection(beaver.CurrentLocation);
                break;
            case Action.Tunnel:
                beaverManager.TheDam.Cells[beaver.CurrentLocation.CellArrayPosition.X, beaver.CurrentLocation.CellArrayPosition.Y].AddConnection(TargetDamCell);
                beaverManager.TheDam.Cells[TargetDamCell.CellArrayPosition.X, TargetDamCell.CellArrayPosition.Y].AddConnection(beaver.CurrentLocation);
                break;
            case Action.Distract:
                //finds all wolves within 5 spaces
                //makes all of them start moving to this square
                break;
        }
    }

    /// <summary>
    /// ToString of an order
    /// </summary>
    /// <returns>What the beaver's doing</returns>
    public override string ToString()
    {
        switch (ThisOrder)
        {
            case Action.Move:
                return $"moving to ({PathToTarget[currentPathIndex].CellCoordinates.Item1}, {PathToTarget[currentPathIndex].CellCoordinates.Item2})";

            case Action.Scavenge:
                return $"scavenging for resources";
            case Action.Barricade:
                return $"barricading between ({beaver.CurrentLocation.CellCoordinates.Item1}, {beaver.CurrentLocation.CellCoordinates.Item2}) and ({TargetDamCell.CellCoordinates.Item1}, {TargetDamCell.CellCoordinates.Item1})";
            case Action.Tunnel:
                return $"tunneling between ({beaver.CurrentLocation.CellCoordinates.Item1}, {beaver.CurrentLocation.CellCoordinates.Item2}) and ({TargetDamCell.CellCoordinates.Item1}, {TargetDamCell.CellCoordinates.Item1})";
            case Action.Distract:
                return "making a racket";
            default:
                return "not doing anything";
        }
    }
}
