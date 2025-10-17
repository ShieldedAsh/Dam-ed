using UnityEngine;
using System.Collections.Generic;
using System;
using System.Drawing;
using UnityEngine.InputSystem.Switch;
using System.Data;

public class DamGenerator : MonoBehaviour
{
    //Variables
    private Stack<DamCell> stack;
    private Point damSize;
    private DamGroup dam;
    private Point hqCoordinate;
    [SerializeField] private int connectionDensityPercentage;
    public static bool hasGenerated = false;

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
    /// <summary>
    /// how much time has passed since the dam was generated
    /// </summary>
    private float time;
    /// <summary>
    /// how many minutes have passed since the dam was generated
    /// </summary>
    private int minutesPassed;

    //Methods
    private void Awake()
    {   
        stack = new Stack<DamCell>();
        time = 0;
        minutesPassed = 0;

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
        dam.SetHQ(dam.Cells[hqCoordinate.X, hqCoordinate.Y]);

        Point start = new Point(hqCoordinate.X - 1, hqCoordinate.Y);
        Point startExtraC = new Point(hqCoordinate.X + 1, hqCoordinate.Y);


        ConnectCells(dam.Cells[hqCoordinate.X, hqCoordinate.Y], new Point(-1, 0));
        ConnectCells(dam.Cells[hqCoordinate.X, hqCoordinate.Y], new Point(1, 0));
        HQ.Instance.SetHQCell(dam.Cells[hqCoordinate.X, hqCoordinate.Y]);

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
        GenerateItems();

        hasGenerated = true;
    }

    float cTime = 0;
    float timeParts = 0;
    private void Update()
    {
        foreach (DamCell cell in dam.Cells)
        {
            bool containsBeaver = false;
            List<BeaverData> references = new List<BeaverData>();
            bool containsWolf = false;
            foreach(IItem item in cell.Contents)
            {
                if (item is BeaverData)
                {
                    containsBeaver = true;
                    references.Add((BeaverData)item);
                }
                if (item is Wolf)
                {
                    containsWolf = true;
                }
            }
            if (containsBeaver && containsWolf)
            {
                for (int i = references.Count - 1; i >= 0; i--)
                {
                    references[i].BeaverStatus = BeaverData.Status.Dead;
                }
            }
        }

        time += Time.deltaTime;
        cTime += Time.deltaTime;
        
        if (cTime >= 10)
        {
            timeParts++;
            cTime -= 10;
            HQ.Instance.EatFood();
            if (timeParts >= 6)
            {
                timeParts -= 6;
                Debug.Log("A minute has passed");
                minutesPassed++;
                GenerateItems();    
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
            foreach(DamCell gCell in dam.Cells)
            {
                foreach (IItem obj in gCell.Contents)
                {
                    switch (obj.itemType)
                    {
                        case IItem.ItemType.Food:
                            Gizmos.color = UnityEngine.Color.green;
                            Gizmos.DrawCube(new Vector3(gCell.CellArrayPosition.X + xOffset - 0.1f, gCell.CellArrayPosition.Y + yOffset - 0.1f, 0), new Vector3(.1f, .1f, .1f));
                            break;
                        case IItem.ItemType.Scrap:
                            Gizmos.color = UnityEngine.Color.black;
                            Gizmos.DrawCube(new Vector3(gCell.CellArrayPosition.X + xOffset + 0.1f, gCell.CellArrayPosition.Y + yOffset - 0.1f, 0), new Vector3(.1f, .1f, .1f));
                            break;
                        case IItem.ItemType.Wolf:
                            Gizmos.color = UnityEngine.Color.grey;
                            Gizmos.DrawCube(new Vector3(gCell.CellArrayPosition.X + xOffset - 0.1f, gCell.CellArrayPosition.Y + yOffset + 0.1f, 0), new Vector3(.1f, .1f, .1f));
                            if (obj is Wolf)
                            {
                                if (((Wolf)obj).CurrentPath != null)
                                {
                                    DamCell current = gCell;
                                    for (int i = ((Wolf)obj).CurrentPath.Count - 1; i >= 0; i--)
                                    {
                                        Gizmos.DrawLine(new Vector3(current.CellArrayPosition.X + xOffset, current.CellArrayPosition.Y + yOffset, 0), new Vector3(((Wolf)obj).CurrentPath[i].CellArrayPosition.X + xOffset, ((Wolf)obj).CurrentPath[i].CellArrayPosition.Y + yOffset, 0));
                                        current = ((Wolf)obj).CurrentPath[i];
                                    }
                                }
                            }
                            break;
                        case IItem.ItemType.Beaver:
                            Gizmos.color = UnityEngine.Color.magenta;
                            Gizmos.DrawCube(new Vector3(gCell.CellArrayPosition.X + xOffset + 0.1f, gCell.CellArrayPosition.Y + yOffset + 0.1f, 0), new Vector3(.1f, .1f, .1f));
                            if (obj is BeaverData)
                            {
                                if (((BeaverData)obj).CurrentOrder != null && ((BeaverData)obj).CurrentOrder.ActionType == Order.Action.Move && ((BeaverData)obj).CurrentOrder.CurrentPath != null && ((BeaverData)obj).CurrentOrder.CurrentPath.Count > 0)
                                {
                                    DamCell current = gCell;
                                    for (int i = ((BeaverData)obj).CurrentOrder.CurrentPath.Count - 1; i >= 0; i--)
                                    {
                                        Gizmos.DrawLine(new Vector3(current.CellArrayPosition.X + xOffset, current.CellArrayPosition.Y + yOffset, 0), new Vector3(((BeaverData)obj).CurrentOrder.CurrentPath[i].CellArrayPosition.X + xOffset, ((BeaverData)obj).CurrentOrder.CurrentPath[i].CellArrayPosition.Y + yOffset, 0));
                                        current = ((BeaverData)obj).CurrentOrder.CurrentPath[i];
                                    }
                                }
                            }
                            break;
                    }
                }
            }

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

                if (gCell.CellArrayPosition == hqCoordinate)
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

    /// <summary>
    /// Generates items on just over 1/4th of the cells
    /// </summary>
    private void GenerateItems()
    {
        int itemCount = (int)((damSize.X * damSize.Y - 1) * .25);
        System.Random RNGesus = new System.Random();
        int debug = 0;
        for(int i = 0; i <= itemCount / 2 + 1; i++)
        {
            DamCell currentCell = dam.Cells[RNGesus.Next(damSize.X), RNGesus.Next(damSize.Y)];
            if(currentCell != dam.Cells[hqCoordinate.X, hqCoordinate.Y] && currentCell.Contents.Count == 0)
            {
                debug++;
                currentCell.AddItem(new Food());
            }
            else
            {
                i--;
            }
        }

        for (int i = 0; i <= itemCount / 2 + 1; i++)
        {
            DamCell currentCell = dam.Cells[RNGesus.Next(damSize.X), RNGesus.Next(damSize.Y)];
            if (currentCell != dam.Cells[hqCoordinate.X, hqCoordinate.Y] && currentCell.Contents.Count == 0)
            {
                debug++;
                currentCell.AddItem(new Scrap());
            }
            else
            {
                i--;
            }
        }
    }

    public void AttemptToRepairConnections()
    {
        List<DamCell> doors = dam.HQ.Connections;

        if (!doors[0].Connections.Contains(dam.HQ))
        {
            doors[0].AddConnection(dam.HQ);
        }

        if (!doors[1].Connections.Contains(dam.HQ))
        {
            doors[1].AddConnection(dam.HQ);
        }
    }
}

