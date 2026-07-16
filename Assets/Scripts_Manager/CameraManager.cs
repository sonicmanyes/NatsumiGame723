using UnityEngine;

public class Scripts_Manager : MonoBehaviour
{
    //追いかける対象
    [Header("移動設定")]
    [SerializeField] private Transform target;

    //カメラの移動範囲を制限
    [SerializeField] private float minX;
    [SerializeField] private float maxX;
    [SerializeField] private float minY;
    [SerializeField] private float maxY;

    [Header("カメラ設定")]
    [SerializeField] private float offsetX = 2f;
    [SerializeField] private float followSpeed = 5f;

    public Transform goalCenterPoint;
    public float goalMoveSpeed = 3f;

    private bool isGoalCamera = false;

    void Update()
    {
        if (isGoalCamera)
        {
            MoveToGoalCenter();
        }
        else
        {
            FollowTarget();
        }
    }

    private void FollowTarget()
    {
        if (target != null)
        {
            // プレイヤーより少し前を映す
            float targetCameraX = target.position.x + offsetX;
            float targetCameraY = target.position.y;

            // カメラの移動範囲を制限
            targetCameraX = Mathf.Clamp(targetCameraX, minX, maxX);
            targetCameraY = Mathf.Clamp(targetCameraY, minY, maxY);

            // なめらかに追いかける
            float x = Mathf.Lerp(transform.position.x, targetCameraX, followSpeed * Time.deltaTime);
            float y = Mathf.Lerp(transform.position.y, targetCameraY, followSpeed * Time.deltaTime);

            // カメラの位置を更新
            transform.position = new Vector3(x, y, transform.position.z);
        }
    }
        
        private void MoveToGoalCenter()
    {
        if (goalCenterPoint == null)//カメラを中央に移動させる
        {
            return;
        }

        float x = Mathf.Lerp(　//Lerpは目的地から少しずつ近づける指示
            transform.position.x,
            goalCenterPoint.position.x,
            goalMoveSpeed * Time.deltaTime　//Time.deltaTimeはPCの性能に関係なく同じ速さで移動する
        );

        float y = Mathf.Lerp(
            transform.position.y,
            goalCenterPoint.position.y,
            goalMoveSpeed * Time.deltaTime
        );

        transform.position = new Vector3(x, y, transform.position.z);
    }
    public void ReturnToCenter()
    {
        isGoalCamera = true;
    }
}