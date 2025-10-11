using UnityEngine;
using System.Collections.Generic;
using System;
using System.Drawing;

public class DamGenerator : MonoBehaviour
{
    //Variables
    private Stack<DamCell> stack;
    private Point damSize;
    private DamGroup dam;
    private Point hqCoordinate;

    //Methods
    private void Awake()
    {
        stack = new Stack<DamCell>();
        
        damSize = new Point(10, 10);

        //Array min 3x3
        if (damSize.X < 3)
        {
            damSize.X = 3;
        }
        if (damSize.Y < 3)
        {
            damSize.Y = 3;
        }
        if (damSize.X % 2 == 0)
        {
            damSize.X++;
        }
        if (damSize.Y % 2 == 0)
        {
            damSize.Y++;
        }

        dam = new DamGroup();
        dam.SetSize(damSize);

        //Fill Grid
        for (int r = 0; r < dam.Cells.GetLength(0); r++)
        {
            for (int c = 0; c < dam.Cells.GetLength(1); c++)
            {
                dam.Cells[r, c] = new DamCell();
                dam.Cells[r, c].SetCoordinate((char)(r + 97), c + 1);
            }
        }
    }

    private void Start()
    {
        
        //Sets HQ
        hqCoordinate = new Point((int)Math.Ceiling(damSize.X / 2.0)-1, (int)Math.Ceiling((damSize.Y / 2.0))-1);
        Point start = new Point(hqCoordinate.X - 1, hqCoordinate.Y);
        Point startExtraC = new Point(hqCoordinate.X + 1, hqCoordinate.Y);

        ConnectCells(dam.Cells[hqCoordinate.X, hqCoordinate.Y], new Point(-1, 0));
        ConnectCells(dam.Cells[hqCoordinate.X, hqCoordinate.Y], new Point(1, 0));

        //Make Initial Connections
        int totalConnections = 3;
        DamCell current = dam.Cells[start.X, start.Y];
        bool abort = false;
        while ((totalConnections < damSize.X * damSize.Y) || abort)
        {
            List<Point> validDirections = new List<Point>();
            //North
            if (current.CellArrayPosition.Y - 1 >= 0 && dam.Cells[current.CellArrayPosition.X, current.CellArrayPosition.Y - 1].Connections.Count == 0)
            {
                validDirections.Add(new Point(0, -1));
            }
            //East
            if (current.CellArrayPosition.X + 1 <= damSize.X - 1 && dam.Cells[current.CellArrayPosition.X + 1, current.CellArrayPosition.Y].Connections.Count == 0)
            {
                validDirections.Add(new Point(1, 0));
            }
            //South
            if (current.CellArrayPosition.Y + 1 <= damSize.Y - 1 && dam.Cells[current.CellArrayPosition.X, current.CellArrayPosition.Y + 1].Connections.Count == 0)
            {
                validDirections.Add(new Point(0, 1));
            }
            //West
            if (current.CellArrayPosition.X - 1 >= 0 && dam.Cells[current.CellArrayPosition.X - 1, current.CellArrayPosition.Y].Connections.Count == 0)
            {
                validDirections.Add(new Point(-1, 0));
            }

            if (validDirections.Count == 0)
            {
                if (stack.Count > 0)
                {
                    current = stack.Pop();
                }
                else
                {
                    abort = true;
                }
            }
            else
            {
                Point nextPoint = validDirections[UnityEngine.Random.Range(0, validDirections.Count)];
                ConnectCells(current, nextPoint);
                stack.Push(current);
                current = dam.Cells[current.CellArrayPosition.X + nextPoint.X, current.CellArrayPosition.Y + nextPoint.Y];
                totalConnections++;
            }
        }

        
        //Make extra connections
        int extraConnections = (int)Math.Round(damSize.X * damSize.Y * .1);
        int connectionsMade = 0;
        
        /*
        //start with cell right of hq
        current = dam.Cells[startExtraC.X, startExtraC.Y];
        switch (UnityEngine.Random.Range(1, 4))
        {
            case 1: // North
                ConnectCells(current, new Point(0, -1));
                break;
            case 2: // East
                ConnectCells(current, new Point(1, 0));
                break;
            case 3: // South
                ConnectCells(current, new Point(0, 1));
                break;
        }
        connectionsMade++;

        //continue on with other cells
        while (connectionsMade < extraConnections)
        {
            current = dam.Cells[UnityEngine.Random.Range(0, damSize.X), UnityEngine.Random.Range(0, damSize.Y)];
            if (current.CellArrayPosition != hqCoordinate)
            {
                int direction = UnityEngine.Random.Range(1, 5);
                if (direction == 1)
                { //North
                    if (current.CellArrayPosition.Y - 1 >= 0 && !dam.Cells[current.CellArrayPosition.X, current.CellArrayPosition.Y - 1].Connections.Contains(current))
                    {
                        ConnectCells(current, new Point(0, -1));
                        connectionsMade++;
                    }
                    else
                    {
                        direction = 2;
                    }
                }
                if (direction == 2)
                { //East
                    if (current.CellArrayPosition.X + 1 <= damSize.X - 1 && !dam.Cells[current.CellArrayPosition.X + 1, current.CellArrayPosition.Y].Connections.Contains(current))
                    {
                        ConnectCells(current, new Point(1, 0));
                        connectionsMade++;
                    }
                    else
                    {
                        direction = 3;
                    }
                }
                if (direction == 3)
                { //South
                    if (current.CellArrayPosition.Y + 1 <= damSize.Y - 1 && !dam.Cells[current.CellArrayPosition.X, current.CellArrayPosition.Y + 1].Connections.Contains(current))
                    {
                        ConnectCells(current, new Point(0, 1));
                        connectionsMade++;
                    }
                    else
                    {
                        direction = 4;
                    }
                }
                if (direction == 4)
                { //West
                    if (current.CellArrayPosition.X - 1 >= 0 && !dam.Cells[current.CellArrayPosition.X - 1, current.CellArrayPosition.Y].Connections.Contains(current))
                    {
                        ConnectCells(current, new Point(-1, 0));
                        connectionsMade++;
                    }
                }
            }
        }
        */
    }

    //Visualize Cells
    private void OnDrawGizmosSelected()
    {
        if (dam != null)
        {
            foreach (DamCell gCell in dam.Cells)
            {
                Gizmos.color = UnityEngine.Color.red;
                Gizmos.DrawWireSphere(new Vector3(gCell.CellArrayPosition.X, gCell.CellArrayPosition.Y, 0), .25f);
                Gizmos.color = UnityEngine.Color.yellow;
                foreach (DamCell cell in gCell.Connections)
                {
                    Gizmos.DrawLine(new Vector3(gCell.CellArrayPosition.X, gCell.CellArrayPosition.Y, 0), new Vector3(cell.CellArrayPosition.X, cell.CellArrayPosition.Y, 0));
                }
            }

            Gizmos.color = UnityEngine.Color.blue;
            Gizmos.DrawWireSphere(new Vector3(hqCoordinate.X, hqCoordinate.Y, 0), .25f);
        }
    }

    private void ConnectCells(DamCell current, Point relativeOffset)
    {
        current.AddConnection(dam.Cells[current.CellArrayPosition.X + relativeOffset.X, current.CellArrayPosition.Y + relativeOffset.Y]);
        dam.Cells[current.CellArrayPosition.X + relativeOffset.X, current.CellArrayPosition.Y + relativeOffset.Y].AddConnection(current);
    }
}
