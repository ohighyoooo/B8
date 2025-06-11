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
            EquipItem(headEquip, "頭部裝備");
        }

        if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            EquipItem(bodyEquip, "身體裝備");
        }

        if (Input.GetKeyDown(KeyCode.Keypad3))
        {
            EquipItem(weaponEquip, "武器裝備");
        }
    }

    void EquipItem(GameObject equipObject, string label)
    {
        if (equipObject != null)
        {
            equipObject.SetActive(true);
            Debug.Log($"P2 裝備了：{label}");
        }

        // 移除地面物件
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
