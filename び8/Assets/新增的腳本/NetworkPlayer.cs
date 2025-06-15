using UnityEngine;

public class NetworkPlayer : MonoBehaviour
{
    [SerializeField]
    Rigidbody rigidbody3d;
    [SerializeField]
    ConfigurableJoint mainJoint;
    public float maxSpeed = 3;

    Vector2 moveInputVector = Vector2.zero;

    bool isJumpButtonPress = false;
    bool isGrounded = false;

    RaycastHit[] raycastHits = new RaycastHit[10];
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        moveInputVector.x = Input.GetAxis("Horizontal");
        moveInputVector.y = Input.GetAxis("Vertical");

        if (Input.GetKeyDown(KeyCode.Space))
        {
            isJumpButtonPress = true;
        }
    }

    void FixedUpdate()
    {
        isGrounded = false;

        int numberOfHit = Physics.SphereCastNonAlloc(rigidbody3d.position, 0.1f, transform.up * -1, raycastHits, 0.5f);

        for (int i = 0; i < numberOfHit; i++)
        {
            //ignore self hit
            if (raycastHits[i].transform.root == transform)
                continue;

            isGrounded = true;

            break;
        }

        if (!isGrounded)
            rigidbody3d.AddForce(Vector3.down * 200);

        float inputMagnitued = moveInputVector.magnitude;// input = ��J�Amagnitude = �W��

        if (inputMagnitued != 0)
        {
            Quaternion desireDirection = Quaternion.LookRotation(new Vector3(moveInputVector.x * -1, 0, moveInputVector.y), transform.up);

            mainJoint.targetRotation = Quaternion.RotateTowards(mainJoint.targetRotation, desireDirection, Time.fixedDeltaTime * 3000);

            Vector3 localVelocifyVersusForward = transform.forward * Vector3.Dot(transform.forward, rigidbody3d.velocity);

            float localForwardVelocity = localVelocifyVersusForward.magnitude;

            if (localForwardVelocity < maxSpeed)
            {
                rigidbody3d.AddForce(transform.forward * inputMagnitued * 600);
            }
        }

        if (isGrounded && isJumpButtonPress)
        {
            rigidbody3d.AddForce(Vector3.up * 500, ForceMode.Impulse);

            isJumpButtonPress = false;
        }
    }//AddForce��rigidbody3d�e�i�A Quaternion.LookRotation�MQuaternion.RotateTowards��mainJoint����A�n�ﲾ�ʪ��ܱqAddForce�MQuaternion�{���X��Ʀr
}
