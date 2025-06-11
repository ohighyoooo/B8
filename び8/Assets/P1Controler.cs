using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody))]
public class P1Controler : MonoBehaviour
{
    public float moveSpeed = 20f;
    private Rigidbody rb;
    public P1ProximityDetector P1proximity;

    public GameObject seaHeadEquip, seaBodyEquip, seaWeaponEquip;
    public GameObject desertHeadEquip, desertBodyEquip, desertWeaponEquip;

    public GameObject seaHeadPrefab, seaBodyPrefab, seaWeaponPrefab;
    public GameObject desertHeadPrefab, desertBodyPrefab, desertWeaponPrefab;

    private bool isPunching = false;
    private bool isBeingKnockedBack = false;
    private bool isStunned = false;

    private bool wasUsingDesertWeapon = false;

    IEnumerator GameOver()
    {
        Debug.Log("沒有裝備可掉落，遊戲結束！");
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(0); // 載入場景編號 0
    }
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        P1proximity = GetComponentInChildren<P1ProximityDetector>();
    }

    void Update()
    {
        if (!isStunned && !isBeingKnockedBack && !isPunching && Input.GetKeyDown(KeyCode.Space))
        {
            if (P1proximity.isPlayer2Nearby)
            {
                GameObject target = P1proximity.targetPlayer;
                if (target != null)
                {
                    P2Controler p2 = target.GetComponent<P2Controler>();
                    if (p2 != null)
                        p2.KnockbackFrom(transform.position);
                }
            }

            Animator animator = GetComponent<Animator>();
            if (animator != null)
            {
                // 根據是否裝備 Desert 武器播放動畫
                if (desertWeaponEquip != null && desertWeaponEquip.activeSelf)
                {
                    animator.SetTrigger("Shoot");
                    StartCoroutine(PunchCooldown(animator.GetCurrentAnimatorStateInfo(0).length)); // 或自訂 shoot 動畫時間
                }
                else
                {
                    animator.SetTrigger("Punch");
                    StartCoroutine(PunchCooldown(animator.GetCurrentAnimatorStateInfo(0).length));
                }
            }
        }


        if (!isStunned && !isBeingKnockedBack)
        {
            HandleMovement();
        }

        HandleEquipInput();

        // 檢查 Desert 武器裝備變化來切換動畫狀態（若未裝備則回復 Punch）
        bool usingDesertWeapon = desertWeaponEquip != null && desertWeaponEquip.activeSelf;
        if (wasUsingDesertWeapon && !usingDesertWeapon)
        {
            Animator animator = GetComponent<Animator>();
            if (animator != null)
            {
                animator.ResetTrigger("Shoot");  // 防止卡在射擊動畫
                animator.ResetTrigger("Punch");  // 重設狀態
                animator.SetTrigger("Punch");    // 回到 Punch 動畫
            }
        }
        wasUsingDesertWeapon = usingDesertWeapon;
    }

    void HandleEquipInput()
    {
        if (P1proximity.nearItem == null) return;

        GameObject item = P1proximity.nearItem;

        if (item.CompareTag("SeaFood"))
        {
            if (Input.GetKeyDown(KeyCode.Y)) EquipItem(seaHeadEquip, seaHeadPrefab, item);
            if (Input.GetKeyDown(KeyCode.U)) EquipItem(seaBodyEquip, seaBodyPrefab, item);
            if (Input.GetKeyDown(KeyCode.I)) EquipItem(seaWeaponEquip, seaWeaponPrefab, item);
        }
        else if (item.CompareTag("Desert"))
        {
            if (Input.GetKeyDown(KeyCode.Y)) EquipItem(desertHeadEquip, desertHeadPrefab, item);
            if (Input.GetKeyDown(KeyCode.U)) EquipItem(desertBodyEquip, desertBodyPrefab, item);
            if (Input.GetKeyDown(KeyCode.I)) EquipItem(desertWeaponEquip, desertWeaponPrefab, item);
        }
    }

    void EquipItem(GameObject equipSlot, GameObject equipPrefab, GameObject groundItem)
    {
        if (equipSlot != null)
        {
            equipSlot.SetActive(false);
        }
        if (equipPrefab != null)
        {
            equipPrefab.SetActive(true);
        }

        if (ItemSpawner.Instance != null)
            ItemSpawner.Instance.HideItem(groundItem);

        P1proximity.nearItem = null;
    }

    void HandleMovement()
    {
        float h = 0, v = 0;
        if (Input.GetKey(KeyCode.A)) h = -1;
        if (Input.GetKey(KeyCode.D)) h = 1;
        if (Input.GetKey(KeyCode.W)) v = 1;
        if (Input.GetKey(KeyCode.S)) v = -1;

        Vector3 dir = new Vector3(h, 0, v).normalized;
        if (dir.magnitude > 0.1f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 0.2f);
            rb.MovePosition(transform.position + dir * moveSpeed * Time.deltaTime);
        }
    }

    IEnumerator PunchCooldown(float duration)
    {
        isPunching = true;
        yield return new WaitForSeconds(duration);
        isPunching = false;
    }

    public void KnockbackFrom(Vector3 sourcePos)
    {
        if (!isBeingKnockedBack)
        {
            StopAllCoroutines();
            StartCoroutine(KnockbackRoutine(sourcePos));
        }
    }

    IEnumerator KnockbackRoutine(Vector3 sourcePos)
    {
        isBeingKnockedBack = true;
        isPunching = false;

        Animator animator = GetComponent<Animator>();
        if (animator)
        {
            animator.ResetTrigger("Punch");
            animator.SetTrigger("GetHit");
        }

        Vector3 dir = (transform.position - sourcePos).normalized;
        dir.y = 0;
        transform.forward = -dir;

        Vector3 start = transform.position;
        Vector3 target = start + dir * 5f;
        float elapsed = 0;

        while (elapsed < 0.5f)
        {
            transform.position = Vector3.Lerp(start, target, elapsed / 0.5f);
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.position = target;
        isBeingKnockedBack = false;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            DropEquippedItem();
            StartCoroutine(StunAndRespawn());
        }
    }

    IEnumerator StunAndRespawn()
    {
        isStunned = true;
        yield return new WaitForSeconds(1.5f);
        transform.position = new Vector3(-20f, 0.98f, 21.9f);
        isStunned = false;
    }

    void DropEquippedItem()
    {
        GameObject itemToDrop = null;

        if (seaWeaponEquip && seaWeaponEquip.activeSelf) { seaWeaponEquip.SetActive(false); itemToDrop = seaWeaponPrefab; }
        else if (seaHeadEquip && seaHeadEquip.activeSelf) { seaHeadEquip.SetActive(false); itemToDrop = seaHeadPrefab; }
        else if (seaBodyEquip && seaBodyEquip.activeSelf) { seaBodyEquip.SetActive(false); itemToDrop = seaBodyPrefab; }
        else if (desertWeaponEquip && desertWeaponEquip.activeSelf) { desertWeaponEquip.SetActive(false); itemToDrop = desertWeaponPrefab; }
        else if (desertHeadEquip && desertHeadEquip.activeSelf) { desertHeadEquip.SetActive(false); itemToDrop = desertHeadPrefab; }
        else if (desertBodyEquip && desertBodyEquip.activeSelf) { desertBodyEquip.SetActive(false); itemToDrop = desertBodyPrefab; }

        if (itemToDrop != null && ItemSpawner.Instance != null)
        {
            ItemSpawner.Instance.DropItem(itemToDrop, transform.position);
        }
        else
        {
            // 沒有任何裝備可掉落，觸發結束
            StartCoroutine(GameOver());
        }
    }
}
