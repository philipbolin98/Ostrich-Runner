using UnityEngine;

public class PlatformScroller : MonoBehaviour {
    private Rigidbody2D rb;
    public float scrollSpeed;

    // Start is called before the first frame update
    void Start() {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = Vector2.left * scrollSpeed;
    }

    public void OnCollisionEnter2D(Collision2D collision) {
        if(collision.gameObject.tag.Equals("Player")) {
            foreach(ContactPoint2D hitPos in collision.contacts) {
                if(hitPos.normal.x == 1) {
                    Destroy(collision.gameObject);
                    GameObject.FindWithTag("GameController").GetComponent<GameController>().GameOver();
                }
            }
        }
    }
}