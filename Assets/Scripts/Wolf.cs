using System.Collections.Generic;
using UnityEditor;
using UnityEditor.VersionControl;
using UnityEngine;
using static UnityEditor.Progress;

public class Wolf : IItem
{
    //TheDam
    private WolfManager wolfManager;
    private DamCell currentLocation;

    //Wolf's Current Location
    public DamCell CurrentLocation { get => currentLocation; set => currentLocation = value; }
    
    //Wolf Pathfinding
    private List<DamCell> pathToTarget;
    private int currentPathIndex;
    private DamCell mainTarget;

    private float timeToMove;

    private bool isDistracted;
    
    /// <summary>
    /// closest cell to mainTarget when trapped
    /// </summary>
    private DamCell intermediateTarget;

    #nullable enable
    public DamCell? MainTarget { get => mainTarget; }
    /// <summary>
    /// This is for viewing the path in DamGenerator
    /// </summary>
    public List<DamCell> CurrentPath { get => pathToTarget; }

    public Wolf(WolfManager wolfManager, DamCell startPosition)
    {
        this.wolfManager = wolfManager;
        timeToMove = Random.Range(2f, 3f);
        currentLocation = startPosition;
        currentLocation.AddItem(this);
        isDistracted = false;
    }

    public void UpdateWolf()
    {
        if (intermediateTarget == null) // is not trapped currently
        {
            if (CurrentLocation.Connections.Contains(HQ.Instance.HQCell) && isDistracted == false) //Wolf attempting to break into HQ (NEEDS TO CHECK IF THERE IS A WOLF AT THE OTHER DOOR)
            {
                HQ hq = HQ.Instance;
                if (CurrentLocation == hq.HQLeftCell) //Checks if the right cell is occupied
                {
                    foreach (IItem item in hq.HQRightCell.Contents)
                    {
                        if (item.itemType == IItem.ItemType.Wolf)
                        {
                            Distract(mainTarget);
                            break;
                        }
                    }
                }
                else if (CurrentLocation == hq.HQRightCell) //Checks if the right cell is occupied
                {
                    foreach (IItem item in hq.HQLeftCell.Contents)
                    {
                        if (item.itemType == IItem.ItemType.Wolf)
                        {
                            Distract(mainTarget);
                            break;
                        }
                    }
                }
                if (timeToMove < 0 && isDistracted == false) //IsDistracted just here to make sure a wolf doesn't sideswipe a door 
                {
                    if (CurrentLocation == hq.HQLeftCell && hq.LeftDoorHealth > 0)
                    {
                        hq.LeftDoorHealth -= 1;
                    }
                    else if (CurrentLocation == hq.HQLeftCell && hq.LeftDoorHealth == 0)
                    {
                        //GAME ENDS AS YOU ARE EATEN
                    }
                    else if (CurrentLocation == hq.HQRightCell && hq.RightDoorHealth > 0)
                    {
                        hq.RightDoorHealth -= 1;
                    }
                    else if (CurrentLocation == hq.HQRightCell && hq.RightDoorHealth == 0)
                    {
                        //GAME ENDS AS YOU ARE EATEN
                    }
                }
            }
            if (CurrentLocation.Connections.Contains(HQ.Instance.HQCell) == false || isDistracted == true)
            //Normal pathfinding (Another if statement to allow above to check to make sure there aren't wolves at both doors without preventing this from running)
            {
                pathToTarget = wolfManager.TheDam.GetShortestPath(CurrentLocation, mainTarget, true);
                // if wolf is trapped
                if (pathToTarget[0].Distance < 0)
                {
                    //Find all cells within isolated chunk
                    List<DamCell> validCells = new List<DamCell>();
                    validCells = GetNeighboringCells(CurrentLocation);

                    //gets all of the cells closest to mainTarget in the isolated chunk
                    List<(DamCell?, float)> shortestDistance = new List<(DamCell?, float)>() { (null, float.MaxValue) };
                    foreach (DamCell cell in validCells)
                    {
                        float dist = Vector2.Distance(new Vector2(cell.CellArrayPosition.X, cell.CellArrayPosition.X), new Vector2(mainTarget.CellArrayPosition.X, mainTarget.CellArrayPosition.Y));
                        if (dist < shortestDistance[0].Item2)
                        {
                            shortestDistance = new List<(DamCell?, float)>() { (cell, dist) };
                        }
                        else if (dist == shortestDistance[0].Item2)
                        {
                            shortestDistance.Add((cell, dist));
                        }
                    }

                    //finds the cell closest to wolf within shortestDistance
                    List<DamCell> shortestPath = wolfManager.TheDam.GetShortestPath(CurrentLocation, shortestDistance[0].Item1, true);
                    intermediateTarget = shortestDistance[0].Item1;
                    for (int i = 1; i < shortestDistance.Count; i++)
                    {
                        List<DamCell> checking = wolfManager.TheDam.GetShortestPath(CurrentLocation, shortestDistance[i].Item1, true);
                        if (checking.Count < shortestPath.Count)
                        {
                            shortestPath = checking;
                            intermediateTarget = shortestDistance[i].Item1;
                            pathToTarget = wolfManager.TheDam.GetShortestPath(CurrentLocation, mainTarget, true);
                        }
                    }

                // if wolf is trapped
                if (pathToTarget[0].Distance < 0)
                {
                    currentLocation.RemoveItem(this);
                    timeToMove = Random.Range(2f, 3f);
                    currentPathIndex = pathToTarget.Count - 1;
                    CurrentLocation = pathToTarget[currentPathIndex - 1];
                    currentLocation.AddItem(this);
                    currentPathIndex--;
                    if (CurrentLocation == mainTarget)
                    {

                    }
                    

                        
                }
                else //Normal movement
                    {
                        if (timeToMove <= 0)
                        {
                            timeToMove = Random.Range(5f, 10f);
                            currentPathIndex = pathToTarget.Count - 1;
                            CurrentLocation = pathToTarget[currentPathIndex - 1];
                            currentPathIndex--;
                            if (CurrentLocation == mainTarget)
                            {
                                mainTarget = null;
                                isDistracted = false;
                            }
                        }
                    }
                }
            }
        }
        else // is trapped
        {
            if (timeToMove <= 0)
            {
                currentLocation.RemoveItem(this);
                timeToMove = Random.Range(2f, 3f);
                pathToTarget = wolfManager.TheDam.GetShortestPath(CurrentLocation, intermediateTarget, true);
                if (pathToTarget[0].Distance < 0)
                {
                    intermediateTarget = null;
                }
                else
                {
                    currentPathIndex = pathToTarget.Count - 1;
                    CurrentLocation = pathToTarget[currentPathIndex - 1];
                    currentLocation.AddItem(this);
                    currentPathIndex--;
                    //when it has reached the intermediate target, dig towards mainTarget
                    if (CurrentLocation == intermediateTarget)
                    {
                        Vector2 digDirection = (new Vector2(mainTarget.CellArrayPosition.X, mainTarget.CellArrayPosition.Y) - new Vector2(CurrentLocation.CellArrayPosition.X, CurrentLocation.CellArrayPosition.Y)).normalized;
                        if (wolfManager.TheDam.Cells[CurrentLocation.CellArrayPosition.X + (int)digDirection.x, CurrentLocation.CellArrayPosition.Y + (int)digDirection.y] == wolfManager.TheDam.HQ)
                        {
                            intermediateTarget = intermediateTarget.Connections[Random.Range(0, intermediateTarget.Connections.Count)];
                        }
                        wolfManager.TheDam.Cells[CurrentLocation.CellArrayPosition.X, CurrentLocation.CellArrayPosition.Y].AddConnection(wolfManager.TheDam.Cells[CurrentLocation.CellArrayPosition.X + (int)digDirection.x, CurrentLocation.CellArrayPosition.Y + (int)digDirection.y]);
                        wolfManager.TheDam.Cells[CurrentLocation.CellArrayPosition.X + (int)digDirection.x, CurrentLocation.CellArrayPosition.Y + (int)digDirection.y].AddConnection(CurrentLocation);
                        intermediateTarget = null;
                    }
                }
            }
        }
        timeToMove -= Time.deltaTime;
    }

    /// <summary>
    /// Recursive function that gets all cells that can be accessed by currentCell
    /// </summary>
    /// <param name="currentCell">The cell the wolf is occupying</param>
    /// <returns>a list of cells accessable by the wolf</returns>
    private List<DamCell> GetNeighboringCells(DamCell currentCell)
    {
        List<DamCell> cells = new List<DamCell>();
        foreach (DamCell cell in currentCell.Connections)
        {
            if (!cells.Contains(cell))
            {
                cells.Add(cell);
                cells.AddRange(GetNeighboringCells(cell));
            }
        }
        return cells;
    }

    /// <summary>
    /// Method for changing the mainTarget of the wolf
    /// </summary>
    /// <param name="target">the cell to move to</param>
    public void ChangeTarget(DamCell target)
    {
        mainTarget = target;
    }

    /// <summary>
    /// Distracts the Wolf towards a target cell
    /// </summary>
    /// <param name="target">The cell the wolf has been distracted towards</param>
    public void Distract(DamCell target)
    {
        ChangeTarget(target);
        isDistracted = true;
    }

    /// <summary>
    /// This is a Wolf item
    /// </summary>
    public IItem.ItemType itemType { get { return IItem.ItemType.Wolf; } }
}
