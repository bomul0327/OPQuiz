using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    /// <summary>
    /// 오브젝트 풀들을 관리할 딕셔너리
    /// </summary>
    public static Dictionary<string, ObjectPool> poolDict = new Dictionary<string, ObjectPool>();
    
    /// <summary>
    /// 첫 프레임에 미리 생성해놓을 수
    /// </summary>
    public int InitialNum;

    /// <summary>
    /// 풀로 사용할 원본 오브젝트
    /// </summary>
    public GameObject origObject;
    
    /// <summary>
    /// 현재 사용하고 있지 않은 오브젝트 리스트
    /// </summary>
    public Queue<PooledObject> unusedObjectQueue;

    /// <summary>
    /// 현재 사용 중인 오브젝트 리스트
    /// </summary>
    public HashSet<PooledObject> usingObjectSet;

    private void Awake()
    {
        unusedObjectQueue = new Queue<PooledObject>();
        usingObjectSet = new HashSet<PooledObject>();

        for (int i = 0; i < InitialNum; ++i)
        {
            var go = Instantiate(origObject, Vector3.zero, Quaternion.identity);
            var po = go.AddComponent<PooledObject>();
            po.SetActive(false);
            unusedObjectQueue.Enqueue(po);
        }
        
        poolDict.Add(origObject.name, this);
    }

    private void OnDestroy()
    {
        origObject = null;

        foreach (var go in usingObjectSet)
        {
            Destroy(go);
        }

        for (int i = 0; i < unusedObjectQueue.Count; ++i)
        {
            var go = unusedObjectQueue.Dequeue();
            Destroy(go);
        }
        
        usingObjectSet.Clear();
        unusedObjectQueue.Clear();

        usingObjectSet = null;
        unusedObjectQueue = null;
    }

    /// <summary>
    /// 사용할 오브젝트 활성화
    /// </summary>
    public PooledObject Create(Vector3 pos, Quaternion rot)
    {
        PooledObject ret;
        
        if (!unusedObjectQueue.TryDequeue(out ret))
        {
            var go = Instantiate(origObject, pos, rot);
            ret = go.AddComponent<PooledObject>();
        }
        else
        {
            ret.transform.SetPositionAndRotation(pos, rot);
            ret.SetActive(true);
        }
        
        usingObjectSet.Add(ret);

        return ret;
    }

    /// <summary>
    /// 사용한 오브젝트 비활성화
    /// </summary>
    public void Return(PooledObject go)
    {
        PooledObject actualGo;
        usingObjectSet.TryGetValue(go, out actualGo);

        if (actualGo)
        {
            actualGo.SetActive(false);
            unusedObjectQueue.Enqueue(actualGo);
            usingObjectSet.Remove(actualGo);
        }
    }

    public static ObjectPool GetPool(string name)
    {
        ObjectPool pool;
        poolDict.TryGetValue(name, out pool);
        
        return pool;
    }
}
