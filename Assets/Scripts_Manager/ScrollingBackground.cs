using UnityEngine;

[RequireComponent(typeof(Camera))]
public class ScrollingBackground : MonoBehaviour
{
    [Header("背景")]
    [SerializeField] private Sprite backgroundSprite;
    [SerializeField] private Transform player;
    [SerializeField] private int sortingOrder = -100;

    [Header("横スクロール")]
    [SerializeField, Min(0f)] private float startDistance = 3f;
    [SerializeField, Min(0f)] private float scrollSpeed = 0.08f;
    [SerializeField, Min(0f)] private float maxScrollOffset = 3f;

    [Header("地面の位置合わせ")]
    [SerializeField, Range(0f, 1f)] private float groundLineFromBottom = 0.12f;
    [SerializeField] private float groundWorldY = -3f;

    private Camera targetCamera;
    private Transform backgroundTransform;
    private float playerStartX;
    private float backgroundLocalY;

    private void Awake()
    {
        targetCamera = GetComponent<Camera>();
        playerStartX = player != null ? player.position.x : 0f;
        CreateBackground();
    }

    private void LateUpdate()
    {
        if (backgroundTransform == null)
        {
            return;
        }

        float scrollOffset = 0f;

        if (player != null)
        {
            float movedDistance = player.position.x - playerStartX;
            float distanceAfterStart = Mathf.Max(0f, movedDistance - startDistance);
            scrollOffset = Mathf.Min(distanceAfterStart * scrollSpeed, maxScrollOffset);
        }

        // カメラには追従しつつ、プレイヤーが進むほど背景だけ少し左へずらす。
        backgroundTransform.localPosition = new Vector3(-scrollOffset, backgroundLocalY, 10f);
    }

    private void CreateBackground()
    {
        if (backgroundSprite == null)
        {
            Debug.LogWarning("背景画像が設定されていません。", this);
            return;
        }

        GameObject backgroundObject = new GameObject("ScrollingStageBackground");
        backgroundTransform = backgroundObject.transform;
        backgroundTransform.SetParent(transform, false);

        SpriteRenderer renderer = backgroundObject.AddComponent<SpriteRenderer>();
        renderer.sprite = backgroundSprite;
        renderer.sortingOrder = sortingOrder;

        FitBackgroundToCamera(renderer);
    }

    private void FitBackgroundToCamera(SpriteRenderer renderer)
    {
        float cameraHeight = targetCamera.orthographicSize * 2f;
        float cameraWidth = cameraHeight * targetCamera.aspect;
        float requiredWidth = cameraWidth + maxScrollOffset * 2f;

        Vector2 spriteSize = renderer.sprite.bounds.size;
        float scale = Mathf.Max(cameraHeight / spriteSize.y, requiredWidth / spriteSize.x);
        backgroundTransform.localScale = new Vector3(scale, scale, 1f);

        // 画像内の草地ラインをゲーム内の地面Y座標へ合わせる。
        float scaledHeight = spriteSize.y * scale;
        float localGroundY = groundWorldY - transform.position.y;
        backgroundLocalY = localGroundY + scaledHeight * (0.5f - groundLineFromBottom);
        backgroundTransform.localPosition = new Vector3(0f, backgroundLocalY, 10f);
    }
}
