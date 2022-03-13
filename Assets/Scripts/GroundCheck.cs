using UnityEngine;

public class GroundCheck : MonoBehaviour {

    private bool isGrounded = false;

    private void OnTriggerStay2D(Collider2D collision) {
        if(collision != null && collision.gameObject.layer == LayerMask.NameToLayer("Ground")) {
            isGrounded = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        isGrounded = false;
    }

    public bool GetIsGrounded() {
        return isGrounded;
    }
}