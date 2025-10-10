using System.Drawing;
using UnityEngine;

public class DamGroup
{
    //Variables
    private DamCell[,] cells;

    //Properties
    //Cells Property
    public DamCell[,] Cells { get => cells; }

    //Constructor
    public DamGroup()
    {
        cells = new DamCell[0, 0];
    }

    //Methods
    public void SetSize(Point size)
    {
        cells = new DamCell[size.X, size.Y];
    }
}
