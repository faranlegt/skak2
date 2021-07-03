using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sea : MonoBehaviour
{
    public static Sea Instance;
    private float _oldZ;

    private void Awake()
    {
        if (Instance)
        {
            Debug.LogError("Sea instance is already set. Deleting duplicate component without game object.");
            Destroy(this);
            return;
        }
        
        Instance = this;
    }

    private void Update()
    {
        float z = transform.position.z;
        if (Mathf.Approximately(_oldZ, z))
        {
            return;
        }
        _oldZ = z;
    }
}
