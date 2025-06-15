using UnityEngine;

public class PrefabSpawner : MonoBehaviour
{
    public GameObject[] prefabs;
    public float spawnInterval = 2f;
    public float spawnRadius = 5f;
    public Vector3 spawnCenter = Vector3.zero;

    private float timer;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            SpawnPrefab();
            timer = 0f;
        }
    }

    void SpawnPrefab()//在project的Assets介面做一個物件prefab，要先把匯入的3d物件拉到場景上，然後再拉到prefab資料夾中，
                      //就可以刪掉場上的物件，就不用把所有要生的物件都留在場景裡
    {
        if (prefabs.Length == 0) return;

        int index = Random.Range(0, prefabs.Length);
        Vector2 randomOffset = Random.insideUnitCircle * spawnRadius;
        Vector3 spawnPos = spawnCenter + new Vector3(randomOffset.x, 0, randomOffset.y);

        GameObject obj = Instantiate(prefabs[index], spawnPos, Quaternion.identity);

        // 確保具備 Rigidbody（可選）
        if (obj.GetComponent<Rigidbody>() == null)
            obj.AddComponent<Rigidbody>();

        // 自動添加 Trigger Collider（若沒有）
        Collider col = obj.GetComponent<Collider>();
        if (col == null)
        {
            col = obj.AddComponent<BoxCollider>();
        }
        col.isTrigger = true;

        // 生成的物件會自動掛上這個自我管理腳本
        if (obj.GetComponent<SpawnedObject>() == null)
            obj.AddComponent<SpawnedObject>();
    }
}



