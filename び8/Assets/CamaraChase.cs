using UnityEngine;

public class CamaraChase : MonoBehaviour
{
    public Transform targetA; // 第一個圓柱
    public Transform targetB; // 第二個圓柱

    public float minZoom = 10f;  // 最近距離
    public float maxZoom = 30f;  // 最遠距離
    public float zoomLimiter = 10f; // 控制縮放的敏感度

    public Vector3 cameraOffset = new Vector3(0f, 30f, -60f); // 角度已設定好，這是俯視角的位置偏移
    public float smoothTime = 0.3f;

    private Vector3 velocity = Vector3.zero;

    void LateUpdate()
    {
        if (targetA == null || targetB == null) return;

        Vector3 centerPoint = GetCenterPoint();
        float distance = Vector3.Distance(targetA.position, targetB.position);

        // 平滑移動到中心點
        Vector3 desiredPosition = centerPoint + cameraOffset;
        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothTime);

        // 計算動態縮放距離（Z軸方向拉遠或拉近）
        float zoom = Mathf.Lerp(minZoom, maxZoom, distance / zoomLimiter);
        transform.position = centerPoint + cameraOffset.normalized * zoom;
    }

    Vector3 GetCenterPoint()
    {
        return (targetA.position + targetB.position) / 2f;
    }
}

// CamaraChase
