using UnityEngine;

public class P1EquipController : MonoBehaviour
{
    // SeaFood �˳�
    public GameObject seaHeadEquip;
    public GameObject seaBodyEquip;
    public GameObject seaWeaponEquip;

    // Desert �˳�
    public GameObject desertHeadEquip;
    public GameObject desertBodyEquip;
    public GameObject desertWeaponEquip;

    private bool isNearItem = false;
    private GameObject nearbyItem;
    private string nearbyItemTag = "";

    void Update()
    {
        if (!isNearItem || nearbyItem == null) return;

        if (Input.GetKeyDown(KeyCode.Y))
        {
            Equip("Head");
        }

        if (Input.GetKeyDown(KeyCode.U))
        {
            Equip("Body");
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            Equip("Weapon");
        }
    }

    void Equip(string part)
    {
        if (nearbyItemTag != "SeaFood" && nearbyItemTag != "Desert") return;

        // �̳�����
        GameObject toEnable = null;
        GameObject toDisable = null;

        if (part == "Head")
        {
            toEnable = (nearbyItemTag == "SeaFood") ? seaHeadEquip : desertHeadEquip;
            toDisable = (nearbyItemTag == "SeaFood") ? desertHeadEquip : seaHeadEquip;
        }
        else if (part == "Body")
        {
            toEnable = (nearbyItemTag == "SeaFood") ? seaBodyEquip : desertBodyEquip;
            toDisable = (nearbyItemTag == "SeaFood") ? desertBodyEquip : seaBodyEquip;
        }
        else if (part == "Weapon")
        {
            toEnable = (nearbyItemTag == "SeaFood") ? seaWeaponEquip : desertWeaponEquip;
            toDisable = (nearbyItemTag == "SeaFood") ? desertWeaponEquip : seaWeaponEquip;
        }

        if (toDisable != null) toDisable.SetActive(false);
        if (toEnable != null) toEnable.SetActive(true);

        Debug.Log($"P1 �˳ƤF�G{nearbyItemTag} - {part}");

        Destroy(nearbyItem);
        isNearItem = false;
        nearbyItem = null;
        nearbyItemTag = "";
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("SeaFood") || other.CompareTag("Desert"))
        {
            isNearItem = true;
            nearbyItem = other.gameObject;
            nearbyItemTag = other.tag;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject == nearbyItem)
        {
            isNearItem = false;
            nearbyItem = null;
            nearbyItemTag = "";
        }
    }
}
