using UnityEngine;

public class P2EquipController : MonoBehaviour
{
    public GameObject headEquip;
    public GameObject bodyEquip;
    public GameObject weaponEquip;

    private bool isNearItem = false;
    private GameObject nearbyItem;

    void Update()
    {
        if (!isNearItem || nearbyItem == null) return;

        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            EquipItem(headEquip, "�Y���˳�");
        }

        if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            EquipItem(bodyEquip, "����˳�");
        }

        if (Input.GetKeyDown(KeyCode.Keypad3))
        {
            EquipItem(weaponEquip, "�Z���˳�");
        }
    }

    void EquipItem(GameObject equipObject, string label)
    {
        if (equipObject != null)
        {
            equipObject.SetActive(true);
            Debug.Log($"P2 �˳ƤF�G{label}");
        }

        // �����a������
        Destroy(nearbyItem);
        isNearItem = false;
        nearbyItem = null;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("SeaFood"))
        {
            isNearItem = true;
            nearbyItem = other.gameObject;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject == nearbyItem)
        {
            isNearItem = false;
            nearbyItem = null;
        }
    }
}
