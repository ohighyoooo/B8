using UnityEngine;

public class SpawnedObject : MonoBehaviour
{
    private bool playerInside = false;
    private float lifetime = 20f; // 物件留著的時間

    void Start()
    {
        Destroy(gameObject, lifetime); // 自動銷毀
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

