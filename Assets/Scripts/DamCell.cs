using UnityEngine;
using System.Collections.Generic;
using System;
using System.Drawing;

public class DamCell
{
    //Variables
    private List<DamCell> connections;
    Tuple<char, int> cellCoordinates;

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

    //Constructor
    public DamCell()
    {
        connections = new List<DamCell>();
        SetCoordinate('a', 1);
    }

    public void SetCoordinate(char row, int col)
    {
        cellCoordinates = new Tuple<char, int>(row, col);
    }

    public void AddConnection(DamCell cell)
    {
        connections.Add(cell);
    }
}
