using UnityEngine;

public class SpawnedObject : MonoBehaviour
{
    private bool playerInside = false;
    private float lifetime = 20f; // ����d�۪��ɶ�

    void Start()
    {
        Destroy(gameObject, lifetime); // �۰ʾP��
    }

    void Update()
    {
        if (playerInside && Input.GetKeyDown(KeyCode.Y))
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player1"))
        {
            playerInside = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player1"))
        {
            playerInside = false;
        }
    }
}

