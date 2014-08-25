using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class PlaneList : MonoBehaviour
{
    public List<Plane> planes = new List<Plane>();

    public void Reset()
    {
        foreach (var plane in planes)
            plane.GetComponent<SelfPoolScript>().PoolObject();
        planes.Clear();
    }

    void Update()
    {

    }
}
