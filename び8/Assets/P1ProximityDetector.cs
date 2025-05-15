using UnityEngine;

public class P1ProximityDetector : MonoBehaviour
{
    public bool isPlayer2Nearby = false;
    public GameObject targetPlayer;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player2"))
        {
            isPlayer2Nearby = true;
            targetPlayer = other.gameObject;
            Debug.Log("P2 �i�J�d��");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player2"))
        {
            isPlayer2Nearby = false;
            targetPlayer = null;
            Debug.Log("P2 ���}�d��");
        }
    }
}
