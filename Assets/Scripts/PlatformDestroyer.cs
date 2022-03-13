using UnityEngine;

//Only to be attached to left boundary
public class PlatformDestroyer : MonoBehaviour {
    private void OnTriggerExit2D(Collider2D collision) {
        Destroy(collision.gameObject);
    }
}