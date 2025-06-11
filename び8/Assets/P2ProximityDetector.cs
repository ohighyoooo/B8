using UnityEngine;

public class P2ProximityDetector : MonoBehaviour
{
    public bool isPlayer1Nearby = false;
    public GameObject targetPlayer;

    public GameObject nearItem;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("SeaFood") || other.CompareTag("Desert"))
        {
            nearItem = other.gameObject;
        }
        if (other.CompareTag("Player1"))
        {
            isPlayer1Nearby = true;
            targetPlayer = other.gameObject;
            Debug.Log("P1 ¶i¤J½d³ò");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (nearItem == other.gameObject)
        {
            nearItem = null;
        }
        if (other.CompareTag("Player1"))
        {
            isPlayer1Nearby = false;
            targetPlayer = null;
            Debug.Log("P1 Â÷¶}½d³ò");
        }
    }
}
