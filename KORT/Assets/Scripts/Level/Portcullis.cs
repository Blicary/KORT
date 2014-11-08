using UnityEngine;
using System.Collections;

public class Portcullis : MonoBehaviour
{
    private Vector3 spawn_point;

    public void Start()
    {
        Transform t = GetComponentInChildren<Transform>();
        if (!t) Debug.LogError("No spawn point object found");
        else spawn_point = t.position;
    }

    public Vector3 GetSpawnPoint()
    {
        return spawn_point;
    }
}
