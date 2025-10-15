using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;

public class Wolf : MonoBehaviour, IItem
{
    //TheDam
    private BeaverManager beaverManager;
    
    //Wolf's Current Location
    public DamCell CurrentLocation { get; set; }
    
    //Wolf Pathfinding
    private List<DamCell> pathToTarget;
    private int currentPathIndex;
    private DamCell mainTarget;
    
    /// <summary>
    /// closest cell to mainTarget when trapped
    /// </summary>
    private DamCell intermediateTarget;

    public void Start()
    {
        beaverManager = FindAnyObjectByType<BeaverManager>();
    }

    public void MoveWolf()
    {
        // this might need to be 0
        if (pathToTarget.Count == 1)
        {
            intermediateTarget = null;
        }

        
        if (intermediateTarget == null) // is not trapped currently
        {
            pathToTarget = beaverManager.TheDam.GetShortestPath(CurrentLocation, mainTarget);
            // if wolf is trapped
            if (pathToTarget[0].Distance <= 0)
            {
                //Find all cells within isolated chunk
                List<DamCell> validCells = new List<DamCell>();
                validCells = GetNeighboringCells(CurrentLocation);

                //gets all of the cells closest to mainTarget in the isolated chunk
                List<(DamCell, float)> shortestDistance = new List<(DamCell, float)>() { (null, float.MaxValue) };
                foreach (DamCell cell in validCells)
                {
                    float dist = Vector2.Distance(new Vector2(cell.CellArrayPosition.X, cell.CellArrayPosition.X), new Vector2(mainTarget.CellArrayPosition.X, mainTarget.CellArrayPosition.Y));
                    if (dist < shortestDistance[0].Item2)
                    {
                        shortestDistance = new List<(DamCell, float)>() { (cell, dist) };
                    }
                    else if (dist == shortestDistance[0].Item2)
                    {
                        shortestDistance.Add((cell, dist));
                    }
                }

                //finds the cell closest to wolf within shortestDistance
                List<DamCell> shortestPath = beaverManager.TheDam.GetShortestPath(CurrentLocation, shortestDistance[0].Item1);
                intermediateTarget = shortestDistance[0].Item1;
                for (int i = 1; i < shortestDistance.Count; i++)
                {
                    List<DamCell> checking = beaverManager.TheDam.GetShortestPath(CurrentLocation, shortestDistance[i].Item1);
                    if (checking.Count < shortestPath.Count)
                    {
                        shortestPath = checking;
                        intermediateTarget = shortestDistance[i].Item1;
                    }
                }
            }
            else //Normal movement
            {
                currentPathIndex = pathToTarget.Count - 1;
                CurrentLocation = pathToTarget[currentPathIndex - 1];
                currentPathIndex--;
            }
        }
        else // is trapped
        {
            pathToTarget = beaverManager.TheDam.GetShortestPath(CurrentLocation, intermediateTarget);
            currentPathIndex = pathToTarget.Count - 1;
            CurrentLocation = pathToTarget[currentPathIndex - 1];
            currentPathIndex--;
            //when it has reached the intermediate target, dig towards mainTarget
            if (CurrentLocation == intermediateTarget)
            {
                Vector2 digDirection = (new Vector2(mainTarget.CellArrayPosition.X, mainTarget.CellArrayPosition.Y) - new Vector2(CurrentLocation.CellArrayPosition.X, CurrentLocation.CellArrayPosition.Y)).normalized;
                beaverManager.TheDam.Cells[CurrentLocation.CellArrayPosition.X, CurrentLocation.CellArrayPosition.Y].AddConnection(beaverManager.TheDam.Cells[CurrentLocation.CellArrayPosition.X + (int)digDirection.x, CurrentLocation.CellArrayPosition.Y + (int)digDirection.y]);
                beaverManager.TheDam.Cells[CurrentLocation.CellArrayPosition.X + (int)digDirection.x, CurrentLocation.CellArrayPosition.Y + (int)digDirection.y].AddConnection(CurrentLocation);
            }
        }
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
    /// This is a Wolf item
    /// </summary>
    public IItem.ItemType itemType { get { return IItem.ItemType.Wolf; } }
}
