using UnityEngine;
using System.Collections;//Coroutineの設定が追加される

public class Door : MonoBehaviour
{
    public Sprite closedDoor;
    public Sprite openDoor;
    public PlayerMove playerMove;
    public Scripts_Manager cameraManager;
    public GameObject Goalpanel;

    private bool playerInDoor = false;
    private SpriteRenderer sr;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = closedDoor;

        Goalpanel.SetActive(false);
    }

    void Update()
    {
        if (GameManager.hasFlower)
        {
            sr.sprite = openDoor;

            if (playerInDoor && Input.GetKeyDown(KeyCode.UpArrow))
            {
                StartCoroutine(GoalSequence());
            }
    }

    }
    private IEnumerator GoalSequence()//〇秒待つという指示。ゴールしてからUI表示を待たせる。
    {
        playerMove.canmove = false;

        cameraManager.ReturnToCenter();

        yield return new WaitForSeconds(0.75f);//ゴールテキスト表示まで指定時間待つ

        Goalpanel.SetActive(true);//GoalUIを表示させる
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInDoor = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInDoor = false;
        }
    }
}
