using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class P1Controler : MonoBehaviour
{
    public float moveSpeed = 20f;
    private Rigidbody rb;
    public P1ProximityDetector P1proximity;

    private bool isPunching = false;
    private bool isBeingKnockedBack = false;
    private bool isStunned = false;

    private Coroutine knockbackCoroutine;
    private Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        P1proximity = GetComponentInChildren<P1ProximityDetector>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (!isStunned && !isBeingKnockedBack && !isPunching && Input.GetKeyDown(KeyCode.Space))
        {
            if (P1proximity.isPlayer2Nearby)
            {
                Debug.Log("���hP2Ĳ�o�I");
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

            // ��������ʵe
            if (animator != null)
            {
                animator.ResetTrigger("GetHit");
                animator.SetTrigger("Punch");
                StartCoroutine(PunchCooldown());
            }
        }

        if (!isStunned && !isBeingKnockedBack)
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

    IEnumerator PunchCooldown()
    {
        isPunching = true;
        // ��ĳ�ϥΰʵe���װѼƱ���
        yield return new WaitForSeconds(0.6f);
        isPunching = false;
    }

    public void KnockbackFrom(Vector3 sourcePosition)
    {
        if (isStunned) return;

        if (knockbackCoroutine != null)
            StopCoroutine(knockbackCoroutine);

        knockbackCoroutine = StartCoroutine(KnockbackRoutine(sourcePosition));
    }

    IEnumerator KnockbackRoutine(Vector3 sourcePosition)
    {
        isBeingKnockedBack = true;
        isPunching = false;

        if (animator != null)
        {
            animator.ResetTrigger("Punch");
            animator.SetTrigger("GetHit");
        }

        Vector3 direction = (transform.position - sourcePosition).normalized;
        direction.y = 0;
        transform.forward = -direction;

        float elapsed = 0f;
        Vector3 start = transform.position;
        Vector3 target = transform.position + direction * 5f;

        while (elapsed < 0.5f)
        {
            if (isStunned) yield break; // �Y���~����A����
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
            if (isBeingKnockedBack || !isStunned)
            {
                Debug.Log("����I�j��w�t�ǰe");
                if (knockbackCoroutine != null)
                    StopCoroutine(knockbackCoroutine);

                StartCoroutine(StunAndRespawn());
            }
        }
    }

    IEnumerator StunAndRespawn()
    {
        isBeingKnockedBack = false;
        isPunching = false;
        isStunned = true;

        // ����ʵe�ʧ@
        if (animator != null)
        {
            animator.ResetTrigger("Punch");
            animator.ResetTrigger("GetHit");
        }

        yield return new WaitForSeconds(1.5f);
        transform.position = new Vector3(-20f, 0.98f, 21.9f);

        isStunned = false;
    }
}
