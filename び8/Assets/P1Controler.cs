using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class P1Controler : MonoBehaviour
{
    public float moveSpeed = 20f;
    public float knockbackDistance = 5f;
    public float knockbackDuration = 0.5f;

    private Rigidbody rb;
    private Animator animator;
    public P1ProximityDetector P1proximity;

    private bool isPunching = false;
    private bool isBeingKnockedBack = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        P1proximity = GetComponentInChildren<P1ProximityDetector>();
    }

    void Update()
    {
        if (!isBeingKnockedBack && !isPunching && Input.GetKeyDown(KeyCode.Space))
        {
            if (P1proximity.isPlayer2Nearby)
            {
                Debug.Log("擊退P2觸發！");
                GameObject target = P1proximity.targetPlayer;
                if (target != null)
                {
                    P2Controler p2 = target.GetComponent<P2Controler>();
                    if (p2 != null)
                    {
                        p2.KnockbackFrom(transform.position);
                    }
                }
            }

            animator.SetTrigger("Punch");
            StartCoroutine(PunchCooldown(animator.GetCurrentAnimatorStateInfo(0).length));
        }

        if (!isBeingKnockedBack)
        {
            HandleMovement();
        }
    }

    void HandleMovement()
    {
        float h = 0;
        float v = 0;

        if (Input.GetKey(KeyCode.A)) h = -1;
        if (Input.GetKey(KeyCode.D)) h = 1;
        if (Input.GetKey(KeyCode.W)) v = 1;
        if (Input.GetKey(KeyCode.S)) v = -1;

        Vector3 direction = new Vector3(h, 0, v).normalized;

        if (direction.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 0.2f);
            rb.MovePosition(transform.position + direction * moveSpeed * Time.deltaTime);
        }
    }

    IEnumerator PunchCooldown(float duration)
    {
        isPunching = true;
        yield return new WaitForSeconds(duration);
        isPunching = false;
    }

    public void KnockbackFrom(Vector3 sourcePosition)
    {
        if (!isBeingKnockedBack)
        {
            StopAllCoroutines(); // 結束 Punch 動畫等
            StartCoroutine(KnockbackRoutine(sourcePosition));
        }
    }

    IEnumerator KnockbackRoutine(Vector3 sourcePosition)
    {
        isBeingKnockedBack = true;
        isPunching = false;

        animator.ResetTrigger("Punch"); // 避免還在打
        animator.SetTrigger("GetHit");

        Vector3 direction = (transform.position - sourcePosition).normalized;
        direction.y = 0;
        transform.forward = -direction;

        float elapsed = 0f;
        Vector3 start = transform.position;
        Vector3 target = transform.position + direction * knockbackDistance;

        while (elapsed < knockbackDuration)
        {
            transform.position = Vector3.Lerp(start, target, elapsed / knockbackDuration);
            Debug.Log("無敵幀");
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = target;
        isBeingKnockedBack = false;
    }
}
