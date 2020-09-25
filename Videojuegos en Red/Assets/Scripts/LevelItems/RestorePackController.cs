using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestorePackController : MonoBehaviour
{
    [SerializeField]
    private int _pointsToRestore;

    public int PointsToRestore { get {return _pointsToRestore ; } private set { _pointsToRestore = value; } }
    // Start is called before the first frame update
    void Start()
    {
        if (_pointsToRestore == 0)
            _pointsToRestore = 1;
    }
}
