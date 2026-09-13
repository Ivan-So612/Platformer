using UnityEngine;
using TMPro;

public class Apple : MonoBehaviour
{

    private TextMeshProUGUI appleText;


    private void Start()
    {
        appleText = GameObject.FindWithTag("AppleText").GetComponent<TextMeshProUGUI>();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Player player = collision.gameObject.GetComponent<Player>();
            player.apples += 1;
            appleText.text = player.apples.ToString();
            Destroy(gameObject);
        }
    }
}
