using UnityEngine;

public class P2ProximityDetector : MonoBehaviour
{
    public bool isPlayer1Nearby = false;
    public GameObject targetPlayer;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player1"))
        {
            isPlayer1Nearby = true;
            targetPlayer = other.gameObject;
            Debug.Log("P1 ¶i¤J½d³ò");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player1"))
        {
            isPlayer1Nearby = false;
            targetPlayer = null;
            Debug.Log("P1 Â÷¶}½d³ò");
        }
    }
}
