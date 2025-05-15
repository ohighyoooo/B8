using UnityEngine;

public class CamaraChase : MonoBehaviour
{
    public Transform targetA; // �Ĥ@�Ӷ�W
    public Transform targetB; // �ĤG�Ӷ�W

    public float minZoom = 10f;  // �̪�Z��
    public float maxZoom = 30f;  // �̻��Z��
    public float zoomLimiter = 10f; // �����Y�񪺱ӷP��

    public Vector3 cameraOffset = new Vector3(0f, 30f, -60f); // ���פw�]�w�n�A�o�O����������m����
    public float smoothTime = 0.3f;

    private Vector3 velocity = Vector3.zero;

    void LateUpdate()
    {
        if (targetA == null || targetB == null) return;

        Vector3 centerPoint = GetCenterPoint();
        float distance = Vector3.Distance(targetA.position, targetB.position);

        // ���Ʋ��ʨ줤���I
        Vector3 desiredPosition = centerPoint + cameraOffset;
        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothTime);

        // �p��ʺA�Y��Z���]Z�b��V�Ի��ΩԪ�^
        float zoom = Mathf.Lerp(minZoom, maxZoom, distance / zoomLimiter);
        transform.position = centerPoint + cameraOffset.normalized * zoom;
    }

    Vector3 GetCenterPoint()
    {
        return (targetA.position + targetB.position) / 2f;
    }
}

// CamaraChase
