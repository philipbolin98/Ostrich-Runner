using UnityEngine;

public class PlatformSpawnTrigger : MonoBehaviour {
    private GameController gameController;

    // Start is called before the first frame update
    private void Start() {
        gameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.gameObject.tag.Equals("Platform")) {
            gameController.SpawnPlatform();
        }
    }
}