using System.Collections.Generic;
using UnityEngine;

public class BeaverManager : MonoBehaviour
{
    [SerializeField] DamGroup theDam;

    public static DamGroup TheDam { get; }

    public List<BeaverData> Beavers { get; private set; }

}
