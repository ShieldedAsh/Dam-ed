
using System.Collections.Generic;
using System.Drawing;
using static UnityEngine.GraphicsBuffer;

public class DamGroup
{
    //Variables
    private DamCell[,] cells;
    private DamCell hq;

    //Properties
    /// <summary>
    /// Read-Only array of DamCells
    /// </summary>
    public DamCell[,] Cells { get => cells; }
    public DamCell HQ { get => hq;}

    //Constructor
    /// <summary>
    /// A dam with no cells
    /// </summary>
    public DamGroup()
    {
        cells = new DamCell[0, 0];
    }

    //Methods
    /// <summary>
    /// Resizes the dam size
    /// </summary>
    /// <param name="size">The dimensions of the dam</param>
    public void SetSize(Point size)
    {
        cells = new DamCell[size.X, size.Y];
    }

    public void GenerateShortestPath(DamCell start, DamCell end)
    {
        DamCell currentCell = start;
        int currentRow = start.CellArrayPosition.X;
        int currentCol = start.CellArrayPosition.Y;
        int endRow = end.CellArrayPosition.X;
        int endCol = end.CellArrayPosition.Y;

        for (int i = 0; i < cells.GetLength(0); i++)
        {
            for (int j = 0; j < cells.GetLength(1); j++)
            {
                cells[i, j].Reset();
            }
        }
        start.Permanence = true;
        start.Distance = 0;

        while (!end.Permanence)
        {
            List<DamCell> adjCells = cells[currentRow, currentCol].Connections;
            for (int k = 0; k < adjCells.Count; k++)
            {
                int cost = int.MaxValue;
                DamCell cell = adjCells[k];
                if (!cell.Permanence)
                {
                    cost = currentCell.Distance + 1;
                    if (cost < cell.Distance)
                    {
                        cell.Distance = cost;
                        cell.PathNeighbor = currentCell;
                    }
                }
            }

            DamCell smallest = null!;
            for (int i = 0; i < cells.GetLength(0); i++)
            {
                for (int j = 0; j < cells.GetLength(1); j++)
                {
                    if (!cells[i,j].Permanence && (smallest == null || cells[i,j].Distance < smallest.Distance))
                    {
                        smallest = cells[i, j];
                        currentRow = i;
                        currentCol = j;
                    }
                }
            }
            smallest.Permanence = true;
            currentCell = smallest;
        }
    }

    /// <summary>
    /// Gets the shortest path to the target
    /// </summary>
    /// <param name="start">Where the path starts</param>
    /// <param name="end">Where the path ends</param>
    /// <returns>A list of DamCells containing the path</returns>
    public List<DamCell> GetShortestPath(DamCell start, DamCell end)
    {
        GenerateShortestPath(start, end);
        DamCell current = end;
        if (current.Distance > 0)
        {
            List<DamCell> path = new List<DamCell>();
            while (current != null)
            {
                path.Add(current);
                if (current.PathNeighbor != null)
                {
                    current = current.PathNeighbor;
                }
                else
                {
                    current = null;
                }
            }
            return path;
        }
        List<DamCell> empty = new List<DamCell>();
        empty.Add(start);
        return empty;
    }

    public void setHQ(DamCell hq)
    {
        this.hq = hq;
    }
}