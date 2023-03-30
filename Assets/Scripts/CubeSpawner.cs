using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CubeSpawner : MonoBehaviour
{
    /// <summary>
    /// 소환할 Cube들
    /// </summary>
    public GameObject CubePrefab;

    /// <summary>
    /// 소환할 위치
    /// </summary>
    public Vector3 SpawnPos;

    /// <summary>
    /// 몇 초마다 생성할 것인지?
    /// </summary>
    [Range(0.001f, 10)]
    public float SpawnTime;

    /// <summary>
    /// 생성된 큐브를 관리하는 리스트
    /// </summary>
    private List<PooledObject> cubeList = new List<PooledObject>();
    ///private List<GameObject> cubeList = new List<GameObject>();

    /// <summary>
    /// 사용할 풀
    /// </summary>
    private ObjectPool cubePool;

    private readonly string CubeString = "Cube";
    
    // Start is called before the first frame update
    void Start()
    {
        // 사용할 오브젝트 풀 불러옴
        cubePool = ObjectPool.GetPool(CubeString);
        StartCoroutine(Timer());
    }

    // Update is called once per frame
    void Update()
    {
        // 키를 누르면 생성 및 제거
        if (Input.GetKey(KeyCode.Space))
        {
            var go = cubePool.Create(SpawnPos, RandomRot());
            //var go = Instantiate(CubePrefab, SpawnPos, RandomRot());
            cubeList.Add(go);
        }
        
        if (Input.GetKey(KeyCode.Return))
        {
            if (cubeList.Count > 0)
            {
                var item = cubeList.Last();
                if (item)
                {
                    cubePool.Return(item);
                    //Destroy(item);
                    cubeList.Remove(item);
                }
            }
        }
    }

    /// <summary>
    /// 일정 시간마다 큐브 생성
    /// </summary>
    /// <returns></returns>
    IEnumerator Timer()
    {
        while (true)
        {
            yield return new WaitForSeconds(SpawnTime);
            var go = cubePool.Create(SpawnPos, RandomRot());
            //var go = Instantiate(CubePrefab, SpawnPos, RandomRot());
            cubeList.Add(go);
        }
    }
    
    Quaternion RandomRot()
    {
        float x = Random.Range(0, 360);
        float y = Random.Range(0, 360);
        float z = Random.Range(0, 360);
        
        Quaternion ret = Quaternion.Euler(x, y, z);
        return ret;
    }
    
}
