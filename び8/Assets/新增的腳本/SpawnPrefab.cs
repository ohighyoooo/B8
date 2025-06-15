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

    void SpawnPrefab()//�bproject��Assets�������@�Ӫ���prefab�A�n����פJ��3d����Ԩ�����W�A�M��A�Ԩ�prefab��Ƨ����A
                      //�N�i�H�R�����W������A�N���Χ�Ҧ��n�ͪ����󳣯d�b������
    {
        if (prefabs.Length == 0) return;

        int index = Random.Range(0, prefabs.Length);
        Vector2 randomOffset = Random.insideUnitCircle * spawnRadius;
        Vector3 spawnPos = spawnCenter + new Vector3(randomOffset.x, 0, randomOffset.y);

        GameObject obj = Instantiate(prefabs[index], spawnPos, Quaternion.identity);

        // �T�O��� Rigidbody�]�i��^
        if (obj.GetComponent<Rigidbody>() == null)
            obj.AddComponent<Rigidbody>();

        // �۰ʲK�[ Trigger Collider�]�Y�S���^
        Collider col = obj.GetComponent<Collider>();
        if (col == null)
        {
            col = obj.AddComponent<BoxCollider>();
        }
        col.isTrigger = true;

        // �ͦ�������|�۰ʱ��W�o�Ӧۧں޲z�}��
        if (obj.GetComponent<SpawnedObject>() == null)
            obj.AddComponent<SpawnedObject>();
    }
}



