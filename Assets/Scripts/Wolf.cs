using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class Wolf : MonoBehaviour, IItem
{
    //TheDam
    private DamGroup theDam;
    public DamGroup TheDam { get => theDam; }
    
    //Wolf's Current Location
    public DamCell CurrentLocation { get; set; }
    
    //Wolf Pathfinding
    private List<DamCell> pathToTarget;
    private int currentPathIndex;

    public void Start()
    {
        theDam = FindAnyObjectByType<DamGenerator>().Dam;
    }

    private void MoveWolf(DamCell start, DamCell end)
    {
        pathToTarget = theDam.GetShortestPath(start, end);
        currentPathIndex = pathToTarget.Count - 1;
        CurrentLocation = pathToTarget[currentPathIndex - 1];
        currentPathIndex--;
    }


    /// <summary>
    /// This is a Wolf item
    /// </summary>
    public IItem.ItemType itemType { get { return IItem.ItemType.Wolf; } }
}
