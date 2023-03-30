using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Object Pool에서 관리될 오브젝트
/// </summary>
public class PooledObject : MonoBehaviour
{
    [HideInInspector]
    public GameObject go;
    
    [HideInInspector]
    public new Transform transform;

    private void Awake()
    {
        go = gameObject;
        transform = GetComponent<Transform>();
    }

    public void SetActive(bool isActive)
    {
        go.SetActive(isActive);
    }
}
