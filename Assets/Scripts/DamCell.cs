using UnityEngine;
using System.Collections.Generic;
using System;
using System.Drawing;
using UnityEngine.UIElements;

#nullable enable
public class DamCell
{
    //Variables
    private List<DamCell> connections;
    private Tuple<char, int> cellCoordinates;

    //Pathfinding Variables
    private bool permanence;
    private int dist;
    private DamCell? pathNeighbor;

    //Properties
    // Converts Ingame Coordinate to array position
    public Point CellArrayPosition
    {
        get
        {
            return new Point((int)cellCoordinates.Item1 - 97, cellCoordinates.Item2 - 1);
        }
    }

    //Ingame Cell Coordinate Property
    public Tuple<char, int> CellCoordinates { get => cellCoordinates; }

    //Connections Property
    public List<DamCell> Connections { get => connections; }


    //Pathfinding Properties
    public bool Permanence { get => permanence; set => permanence = value; }
    public int Distance { get => dist; set => dist = value; }

    public DamCell? PathNeighbor { get => pathNeighbor; set => pathNeighbor = value; }

    //Constructor
    /// <summary>
    /// A cell with no neighbors and default coordinates
    /// </summary>
    public DamCell()
    {
        connections = new List<DamCell>();
        SetCoordinate('a', 1);
        Reset();
    }

    /// <summary>
    /// assigns in-game coordinates, ex. a1
    /// </summary>
    /// <param name="row">the character for the row (a = 1)</param>
    /// <param name="col">the column</param>
    public void SetCoordinate(char row, int col)
    {
        cellCoordinates = new Tuple<char, int>(row, col);
    }

    /// <summary>
    /// Adds a one-way connection to a cell
    /// </summary>
    /// <param name="cell">The cell to connect to</param>
    public void AddConnection(DamCell cell)
    {
        connections.Add(cell);
    }

    /// <summary>
    /// Resets the pathfinding variables for the cell
    /// </summary>
    public void Reset()
    {
        permanence = false;
        dist = int.MaxValue;
        pathNeighbor = null;
    }
}
