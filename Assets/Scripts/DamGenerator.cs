using UnityEngine;
using System.Collections.Generic;
using System;
using System.Drawing;
using UnityEngine.InputSystem.Switch;

public class DamGenerator : MonoBehaviour
{
    //Variables
    private Stack<DamCell> stack;
    private Point damSize;
    private DamGroup dam;
    private Point hqCoordinate;
    [SerializeField] private int connectionDensityPercentage;
    public static bool hasGenerated = false;

    //Path Testing
    private Vector2 startIndex;
    private Vector2 endIndex;
    [SerializeField] private bool updateEachFrame;


    //Properties
    /// <summary>
    /// The dimensions of the dam
    /// </summary>
    /// Points don't show up in the editor
    public Vector2 DamSize;
    
    /// <summary>
    /// The percentage of extra connections made (max 22)
    /// </summary>
    public int ConnectionDensityPercentage { get => connectionDensityPercentage; set => connectionDensityPercentage = value; }

    public DamGroup Dam { get => dam; set => dam = value; }

    //Drawing Properties
    public UnityEngine.Color connectionColor;
    public Sprite cellSprite;
    private GameObject child;
    private GameObject connection;
    private LineRenderer connectionLine;
    public float connectionWidth;
    public Sprite maskSprite;
    //Methods
    private void Awake()
    {   
        stack = new Stack<DamCell>();

        //Converts damSize from Vector2 --> Point
        damSize = new Point((int)DamSize.x, (int)DamSize.y);

        /* Prevents issues with the algorithm
         * 5x5 min grid
         * Grid has odd numbered dimensions
         */
        if (damSize.X < 5)
        {
            damSize.X = 5;
        }
        if (damSize.Y < 5)
        {
            damSize.Y = 5;
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

        //Sets HQ
        hqCoordinate = new Point((int)Math.Ceiling(damSize.X / 2.0) - 1, (int)Math.Ceiling((damSize.Y / 2.0)) - 1);
        dam.setHQ(dam.Cells[hqCoordinate.X, hqCoordinate.Y]);

        startIndex = new Vector2(hqCoordinate.X, hqCoordinate.Y);
        Point start = new Point(hqCoordinate.X - 1, hqCoordinate.Y);
        Point startExtraC = new Point(hqCoordinate.X + 1, hqCoordinate.Y);


        ConnectCells(dam.Cells[hqCoordinate.X, hqCoordinate.Y], new Point(-1, 0));
        ConnectCells(dam.Cells[hqCoordinate.X, hqCoordinate.Y], new Point(1, 0));

        //Make Initial Connections
        int totalConnections = 3;
        DamCell current = dam.Cells[start.X, start.Y];
        while (totalConnections < damSize.X * damSize.Y)
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

        /* 3x3 grids can only have 2 extra connections be made 
         * with how the algorithm works, the amount of extra connections allowed is cellCount * cDP
         * so 2/9 is the max that cDP can be so as an int it goes to 22
         */
        if (connectionDensityPercentage > 22)
        {
            connectionDensityPercentage = 22;
        }
        if (connectionDensityPercentage < 5)
        {
            connectionDensityPercentage = 5;
        }
        int extraConnections = (int)Math.Round(damSize.X * damSize.Y * (connectionDensityPercentage / 100.0));
        int connectionsMade = 0;


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

        //connect other cells
        while (connectionsMade < extraConnections)
        {
            current = dam.Cells[UnityEngine.Random.Range(0, damSize.X), UnityEngine.Random.Range(0, damSize.Y)];
            if (current.CellArrayPosition != hqCoordinate)
            {
                List<Point> validDirections = new List<Point>();
                //North
                if (current.CellArrayPosition.Y - 1 >= 0 && !dam.Cells[current.CellArrayPosition.X, current.CellArrayPosition.Y - 1].Connections.Contains(current))
                {
                    validDirections.Add(new Point(0, -1));
                }
                //East
                if (current.CellArrayPosition.X + 1 <= damSize.X - 1 && !dam.Cells[current.CellArrayPosition.X + 1, current.CellArrayPosition.Y].Connections.Contains(current))
                {
                    validDirections.Add(new Point(1, 0));
                }
                //South
                if (current.CellArrayPosition.Y + 1 <= damSize.Y - 1 && !dam.Cells[current.CellArrayPosition.X, current.CellArrayPosition.Y + 1].Connections.Contains(current))
                {
                    validDirections.Add(new Point(0, 1));
                }
                //West
                if (current.CellArrayPosition.X - 1 >= 0 && !dam.Cells[current.CellArrayPosition.X - 1, current.CellArrayPosition.Y].Connections.Contains(current))
                {
                    validDirections.Add(new Point(-1, 0));
                }

                if (validDirections.Count != 0)
                {
                    Point nextPoint = validDirections[UnityEngine.Random.Range(0, validDirections.Count)];
                    ConnectCells(current, nextPoint);
                    connectionsMade++;
                }
            }
        }

        DrawDam();
        hasGenerated = true;
    }

    private void Update()
    {
        if (updateEachFrame || Input.GetKeyDown(KeyCode.Space))
        {
            endIndex.x++;
            if (endIndex.x >= damSize.X)
            {
                endIndex.x = 0;
                endIndex.y++;
            }
            if (endIndex.y >= damSize.Y)
            {
                endIndex.y = 0;
            }
        }
    }

    //Visualizes the dam, drawn position is based off parent transform
    private void OnDrawGizmosSelected()
    {
        /* Cells are red circles
        * Connections are yellow lines
        * HQ is a blue circle
        * Pathfinding from HQ --> endIndex are green lines
        */
        if (dam != null)
        {
            float xOffset = transform.position.x - hqCoordinate.X;
            float yOffset = transform.position.y - hqCoordinate.Y;
            foreach (DamCell gCell in dam.Cells)
            {
                Gizmos.color = UnityEngine.Color.red;
                Gizmos.DrawWireSphere(new Vector3(gCell.CellArrayPosition.X + xOffset, gCell.CellArrayPosition.Y + yOffset, 0), .25f);
                Gizmos.color = UnityEngine.Color.yellow;
                foreach (DamCell cell in gCell.Connections)
                {
                    Gizmos.DrawLine(new Vector3(gCell.CellArrayPosition.X + xOffset, gCell.CellArrayPosition.Y + yOffset, 0), new Vector3(cell.CellArrayPosition.X + xOffset, cell.CellArrayPosition.Y + yOffset, 0));
                }
            }

            /*dam.GenerateShortestPath(dam.Cells[(int)startIndex.x, (int)startIndex.y], dam.Cells[(int)endIndex.x, (int)endIndex.y]);
            
            DamCell current = dam.Cells[(int)endIndex.x, (int)endIndex.y];
            while (current != null)
            {
                Gizmos.color = UnityEngine.Color.green;
                Gizmos.DrawWireSphere(new Vector3(current.CellArrayPosition.X + xOffset, current.CellArrayPosition.Y + yOffset, 0), .25f);
                if (current.PathNeighbor != null)
                {
                    Gizmos.DrawLine(new Vector3(current.CellArrayPosition.X + xOffset, current.CellArrayPosition.Y + yOffset, 0), new Vector3(current.PathNeighbor.CellArrayPosition.X + xOffset, current.PathNeighbor.CellArrayPosition.Y + yOffset, 0));
                }
                current = current.PathNeighbor;
            }*/

            Gizmos.color = UnityEngine.Color.blue;
            Gizmos.DrawWireSphere(transform.position, .25f);
        }
    }

    /// <summary>
    /// Makes a two way connection between two points
    /// </summary>
    /// <param name="current">The first DamCell being connected</param>
    /// <param name="relativeOffset">The coordinates relative to "current", ex. (1,0) would be right/east</param>
    private void ConnectCells(DamCell current, Point relativeOffset)
    {
        current.AddConnection(dam.Cells[current.CellArrayPosition.X + relativeOffset.X, current.CellArrayPosition.Y + relativeOffset.Y]);
        dam.Cells[current.CellArrayPosition.X + relativeOffset.X, current.CellArrayPosition.Y + relativeOffset.Y].AddConnection(current);
    }
    

    private void DrawDam()
    {
        if (dam != null)
        {

            float xOffset = transform.position.x - hqCoordinate.X;
            float yOffset = transform.position.y - hqCoordinate.Y;
            foreach (DamCell gCell in dam.Cells)
            {
                child = new GameObject();
                child.transform.SetParent(transform);
                child.transform.position = new Vector3(gCell.CellArrayPosition.X + xOffset, gCell.CellArrayPosition.Y + yOffset, 0);
                child.AddComponent<SpriteRenderer>();
                child.GetComponent<SpriteRenderer>().sprite = cellSprite;
                child.GetComponent<SpriteRenderer>().sortingLayerName = "Map";
                child.GetComponent<SpriteRenderer>().color = UnityEngine.Color.black;
                child.AddComponent<SpriteMask>();
                child.GetComponent<SpriteMask>().sprite = maskSprite;
                child.transform.name = gCell.CellArrayPosition.ToString();
                
                if(gCell.CellArrayPosition == hqCoordinate)
                {
                    child.GetComponent<SpriteRenderer>().color = UnityEngine.Color.red;
                }


                foreach (DamCell cell in gCell.Connections)
                {
                    connection = new GameObject();
                    connection.transform.SetParent(child.transform);
                    connectionLine = connection.AddComponent<LineRenderer>();
                    connectionLine.material = new Material(Shader.Find("Sprites/Default"));
                    connectionLine.startWidth = connectionWidth;
                    connectionLine.endWidth = connectionWidth;
                    connectionLine.startColor = connectionColor;
                    connectionLine.endColor = connectionColor;
                    connectionLine.sortingLayerName = "Map";
                    connectionLine.sortingOrder = -1;
                    connectionLine.positionCount = 2;


                    connectionLine.SetPosition(0, new Vector3(gCell.CellArrayPosition.X + xOffset, gCell.CellArrayPosition.Y + yOffset, 0));
                    connectionLine.SetPosition(1, new Vector3(cell.CellArrayPosition.X + xOffset, cell.CellArrayPosition.Y + yOffset, 0));
                    connectionLine.maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;
                    
                }
            }
        }
    }
}
