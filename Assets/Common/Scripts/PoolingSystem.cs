using System.Collections.Generic;
using UnityEngine;

public static class PoolingSystem
{
    private static Dictionary<int, Queue<GameObject>> PoolDictionary = new Dictionary<int, Queue<GameObject>>();

    public static T Spawn<T>(GameObject prefab, Transform parent, Vector3 localScale, Vector3 localPosition, Quaternion localRotation)
        where T : Component
    {
        int id = prefab.GetInstanceID();

        if (PoolDictionary.ContainsKey(id) == false)
        {
            PoolDictionary[id] = new Queue<GameObject>();
        }

        GameObject instance;

        if (PoolDictionary[id].Count > 0)
        {
            instance = PoolDictionary[id].Dequeue();
        }
        else
        {
            instance = Object.Instantiate(prefab);
        }

        instance.SetActive(true);
        instance.transform.SetParent(parent);
        instance.transform.localScale = localScale;
        instance.transform.SetLocalPositionAndRotation(localPosition, localRotation);
        return instance.GetComponent<T>();
    }

    public static void Despawn(GameObject prefab, GameObject instance)
    {
        int id = prefab.GetInstanceID();

        if (PoolDictionary.ContainsKey(id) && instance.activeSelf)
        {
            PoolDictionary[id].Enqueue(instance);
            instance.SetActive(false);
        }
    }
}
