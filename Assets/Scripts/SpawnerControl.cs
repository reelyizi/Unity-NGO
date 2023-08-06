using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class SpawnerControl : NetworkSingleton<SpawnerControl>
{
    [SerializeField] private GameObject objPrefab;
    [SerializeField] private int maxObjInstanceCount = 3;

    private void Start()
    {
        NetworkManager.Singleton.OnServerStarted += () =>
        {
            NetworkObjectPool.Instance.OnNetworkSpawn();
        };
    }

    public void SpawnObjects()
    {
        if (!IsServer) return;

        for (int i = 0; i < maxObjInstanceCount; i++)
        {
            //GameObject go = Instantiate(objPrefab, new Vector3(Random.Range(-10, 10), 10, Random.Range(-10, 10)), Quaternion.identity);

            GameObject go = NetworkObjectPool.Instance.GetNetworkObject(objPrefab, new Vector3(Random.Range(-10, 10), 10, Random.Range(-10, 10)), Quaternion.identity).gameObject;

            go.GetComponent<Rigidbody>().isKinematic = false;
            go.GetComponent<NetworkObject>().Spawn();

        }
    }
}
