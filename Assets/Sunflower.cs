
using UnityEngine;

public class Sunflower : MonoBehaviour
{
    public AudioClip getSE;
    private void OnTriggerEnter2D(Collider2D other)　//このオブジェクト意外がぶつかって来た時
    {
        if (other.CompareTag("Player")) //なつみさんのタグをPlayerに設定する
        {
            GameManager.hasFlower = true;

            AudioSource audio = other.GetComponent<AudioSource>();
            audio.PlayOneShot(getSE);

            Destroy(gameObject);　//最初にSunflowerと宣言してるからgameobjectでいい
        }
    }
}