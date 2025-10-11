using System.Drawing;
using UnityEngine;

public class DamGroup
{
    //Variables
    private DamCell[,] cells;

    //Properties
    /// <summary>
    /// Read-Only array of DamCells
    /// </summary>
    public DamCell[,] Cells { get => cells; }

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
}
