using UnityEngine;

[DefaultExecutionOrder(-100)]
[RequireComponent(typeof(Camera))]
public class FixedCameraY : MonoBehaviour
{
    [Header("固定するカメラ設定")]
    [SerializeField] private float fixedY = 0f;
    [SerializeField, Min(0.1f)] private float orthographicSize = 8f;

    private Camera targetCamera;

    private void Awake()
    {
        targetCamera = GetComponent<Camera>();
        targetCamera.orthographicSize = orthographicSize;
        FixVerticalPosition();
    }

    private void LateUpdate()
    {
        FixVerticalPosition();
    }

    private void FixVerticalPosition()
    {
        Vector3 position = transform.position;
        position.y = fixedY;
        transform.position = position;
    }
}
