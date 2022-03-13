using UnityEngine;

public class DestroyByContact : MonoBehaviour {
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {
            Destroy(collision.gameObject);
            GameObject.FindWithTag("GameController").GetComponent<GameController>().GameOver();
        }
    }
}