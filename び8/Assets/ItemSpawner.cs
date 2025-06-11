using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemSpawner : MonoBehaviour
{
    public static ItemSpawner Instance;

    public List<GameObject> itemPrefabs; // �w���t�m�n���a�W���� prefab
    private List<GameObject> pooledItems = new List<GameObject>();

    private float spawnRadius = 53f;
    private Vector3 centerPosition = new Vector3(-29f, 1f, 0f);

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Instance = this; // ���� Destroy�A�קK�~�R
    }

    private void Start()
    {
        // ��l�ƪ����
        foreach (var prefab in itemPrefabs)
        {
            GameObject item = Instantiate(prefab);
            item.SetActive(false);
            pooledItems.Add(item);
        }

        // ��l�ͦ� 2 �Ӫ���
        SpawnRandomItem();
        SpawnRandomItem();

        StartCoroutine(RespawnCycle());
    }

    IEnumerator RespawnCycle()
    {
        while (true)
        {
            yield return new WaitUntil(() => !IsAnyItemActive());
            yield return new WaitForSeconds(Random.Range(10f, 20f));
            SpawnRandomItem();
        }
    }

    void SpawnRandomItem()
    {
        GameObject item = GetInactiveItem();
        if (item != null)
        {
            item.transform.position = GetRandomPosition();
            item.SetActive(true);
            StartCoroutine(AutoHide(item, 10f));
        }
    }

    IEnumerator AutoHide(GameObject item, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (item != null && item.activeSelf)
        {
            item.SetActive(false);
            StartCoroutine(RespawnAfterDelay(item, Random.Range(10f, 20f)));
        }
    }

    IEnumerator RespawnAfterDelay(GameObject item, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (item != null && !item.activeSelf)
        {
            item.transform.position = GetRandomPosition();
            item.SetActive(true);
            StartCoroutine(AutoHide(item, 10f)); // ���s�Ұʴ`��
        }
    }

    bool IsAnyItemActive()
    {
        pooledItems.RemoveAll(item => item == null); // �M���w�g�Q�N�~�P��������
        foreach (var item in pooledItems)
        {
            if (item != null && item.activeSelf)
                return true;
        }
        return false;
    }

    GameObject GetInactiveItem()
    {
        foreach (var item in pooledItems)
        {
            if (item != null && !item.activeSelf)
                return item;
        }
        return null;
    }

    Vector3 GetRandomPosition()
    {
        Vector2 randCircle = Random.insideUnitCircle * spawnRadius;
        return new Vector3(centerPosition.x + randCircle.x, centerPosition.y, centerPosition.z + randCircle.y);
    }

    public void HideItem(GameObject item)
    {
        if (item != null && item.activeSelf)
        {
            item.SetActive(false);
            StartCoroutine(RespawnAfterDelay(item, Random.Range(10f, 20f)));
        }
    }

    public void DropItem(GameObject prefab, Vector3 fromPosition)
    {
        GameObject item = GetInactiveItem();
        if (item != null)
        {
            item.transform.position = FindClosestDropPosition(fromPosition);
            item.SetActive(true);
            StartCoroutine(AutoHide(item, 10f));
        }
    }

    Vector3 FindClosestDropPosition(Vector3 origin)
    {
        Vector2 dir = new Vector2(origin.x - centerPosition.x, origin.z - centerPosition.z);
        dir = Vector2.ClampMagnitude(dir, spawnRadius);
        return new Vector3(centerPosition.x + dir.x, centerPosition.y, centerPosition.z + dir.y);
    }
}
