using UnityEngine;

[RequireComponent(typeof(Camera))]
public class ScrollingBackground : MonoBehaviour
{
    [Header("背景")]
    [SerializeField] private Sprite backgroundSprite;
    [SerializeField] private Transform player;
    [SerializeField] private int sortingOrder = -100;

    [Header("スクロール設定")]
    [SerializeField, Min(0f)] private float startDistance = 3f;
    [SerializeField, Min(0f)] private float scrollSpeed = 0.08f;
    [SerializeField, Min(0f)] private float maxScrollOffset = 3f;

    private Camera targetCamera;
    private Transform backgroundTransform;
    private float playerStartX;

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
        backgroundTransform.localPosition = new Vector3(-scrollOffset, 0f, 10f);
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
    }
}
