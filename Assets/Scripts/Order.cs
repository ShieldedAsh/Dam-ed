using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Order
{
    private List<DamCell> pathToTarget;
    private int currentPathIndex;

    /// <summary>
    /// This is for viewing the path in DamGenerator
    /// </summary>
    public List<DamCell> CurrentPath { get => pathToTarget; }

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
    /// This is for viewing the path in DamGenerator
    /// </summary>
    public Action ActionType { get => action; }

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
        TargetDamCell = target;
        if (ThisOrder == Action.Move)
        {
            pathToTarget = beaverManager.TheDam.GetShortestPath(beaver.CurrentLocation, TargetDamCell, false);
            currentPathIndex = pathToTarget.Count - 1;
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
                pathToTarget = beaverManager.TheDam.GetShortestPath(beaver.CurrentLocation, TargetDamCell, false);
                beaver.CurrentLocation.RemoveItem(beaver);
                currentPathIndex = pathToTarget.Count - 1;
                beaver.CurrentLocation = pathToTarget[currentPathIndex - 1];
                beaver.CurrentLocation.AddItem(beaver);
                currentPathIndex--;
                break;
            case Action.Scavenge:
                if(beaver.CurrentLocation.Contents.Count != 0) //makes sure there are items on this cell
                {
                    beaver.Carrying = beaver.CurrentLocation.Contents[beaver.CurrentLocation.Contents.Count - 1];
                    beaver.CurrentLocation.Contents.RemoveAt(beaver.CurrentLocation.Contents.Count - 1);
                }
                break;
            case Action.Barricade:
                if(beaverManager.TheDam.HQ == beaver.CurrentLocation && beaver.Carrying.itemType == IItem.ItemType.Scrap) //Checks if the beaver is currently in the HQ to try to reinforce the doors and has scrap
                {
                    HQ hq = HQ.Instance;
                    if (TargetDamCell == hq.HQLeftCell)
                    {
                        hq.LeftDoorHealth += 1;
                        beaver.Carrying = null;
                    }
                    else if (TargetDamCell == hq.HQRightCell)
                    {
                        hq.RightDoorHealth += 1;
                        beaver.Carrying = null;
                    }
                }
                else if (beaver.Carrying.itemType == IItem.ItemType.Scrap && IsNeighbor(beaver.CurrentLocation, TargetDamCell) && beaver.CurrentLocation.Connections.Contains(TargetDamCell)) //Checks if the beaver has scrap to barricade with
                {
                    beaverManager.TheDam.Cells[beaver.CurrentLocation.CellArrayPosition.X, beaver.CurrentLocation.CellArrayPosition.Y].RemoveConnection(TargetDamCell);
                    beaverManager.TheDam.Cells[TargetDamCell.CellArrayPosition.X, TargetDamCell.CellArrayPosition.Y].RemoveConnection(beaver.CurrentLocation);
                    beaver.Carrying = null;
                }
                break;
            case Action.Tunnel:
                if (beaver.Carrying.itemType == IItem.ItemType.Scrap && IsNeighbor(beaver.CurrentLocation, TargetDamCell) && !beaver.CurrentLocation.Connections.Contains(TargetDamCell) //Checks all logic for tunneling
                    && beaverManager.TheDam.HQ != TargetDamCell && beaverManager.TheDam.HQ != beaver.CurrentLocation)
                {
                    beaverManager.TheDam.Cells[beaver.CurrentLocation.CellArrayPosition.X, beaver.CurrentLocation.CellArrayPosition.Y].AddConnection(TargetDamCell);
                    beaverManager.TheDam.Cells[TargetDamCell.CellArrayPosition.X, TargetDamCell.CellArrayPosition.Y].AddConnection(beaver.CurrentLocation);
                    beaver.Carrying = null;
                }
                break;
            case Action.Distract:
                List<DamCell> distractCells = new List<DamCell>(); //Cells that wolves can hear from
                distractCells.Add(beaver.CurrentLocation);
                for(int i = 0; i < 5; i++) //gets every cell within 5 cells of the current cell
                {
                    List<DamCell> toAdd = new List<DamCell>();
                    foreach(DamCell cell in distractCells)
                    {
                        foreach(DamCell connectCell in cell.Connections)
                        {
                            if(distractCells.Contains(connectCell) == false && toAdd.Contains(connectCell) == false)
                            {
                                toAdd.Add(connectCell);
                            }
                        }
                    }
                    foreach (DamCell cell in distractCells)
                    {
                        distractCells.Add(cell);
                    }
                }

                foreach (Wolf wolf in Object.FindAnyObjectByType<WolfManager>().Wolves) //Distracts all wolves in the area
                {
                    foreach (DamCell cell in distractCells)
                    {
                        if (wolf.CurrentLocation == cell)
                        {
                            wolf.Distract(beaver.CurrentLocation);
                            break;
                        }
                    }
                }
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
                return $"moving to ({pathToTarget[0].CellCoordinates.Item1}, {pathToTarget[0].CellCoordinates.Item2})";

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
    
    /// <summary>
    /// Checks to see if target is a neighbor to currentLocation
    /// </summary>
    /// <param name="currentLocation">The current location</param>
    /// <param name="target">the cell to check</param>
    /// <returns>True if a neighbor, false if not</returns>
    public bool IsNeighbor(DamCell currentLocation, DamCell target)
    {
        // Y-1
        if (currentLocation.CellArrayPosition.Y-1 >= 0)
        {
            if (currentLocation.CellArrayPosition.X == target.CellArrayPosition.X && currentLocation.CellArrayPosition.Y - 1 == target.CellArrayPosition.Y)
            {
                return true;
            }
        }
        // X-1
        if (currentLocation.CellArrayPosition.X-1 >= 0)
        {
            if (currentLocation.CellArrayPosition.X-1 == target.CellArrayPosition.X && currentLocation.CellArrayPosition.Y == target.CellArrayPosition.Y)
            {
                return true;
            }
        }
        // Y+1
        if (currentLocation.CellArrayPosition.Y+1 < beaverManager.TheDam.Cells.GetLength(1))
        {
            if (currentLocation.CellArrayPosition.X == target.CellArrayPosition.X && currentLocation.CellArrayPosition.Y + 1 == target.CellArrayPosition.Y)
            {
                return true;
            }
        }
        // X+1
        if (currentLocation.CellArrayPosition.X + 1 < beaverManager.TheDam.Cells.GetLength(0))
        {
            if (currentLocation.CellArrayPosition.X + 1 == target.CellArrayPosition.X && currentLocation.CellArrayPosition.Y == target.CellArrayPosition.Y)
            {
                return true;
            }
        }
        return false;

    }
}
