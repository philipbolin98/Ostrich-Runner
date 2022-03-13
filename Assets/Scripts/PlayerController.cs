using UnityEngine;

public class PlayerController : MonoBehaviour {
    private Rigidbody2D rb;
    private Animator animator;
    private bool isJumping = false;
    private bool gameStarted = false;
    private float jumpTimeCounter = 0;

    public GameObject readyText;
    public GameObject gameController;
    public float jumpForce;
    public float jumpTime;
    public float defaultGrav;
    public float glidingGrav;

    // Start is called before the first frame update
    void Start() {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update() {
        if (gameStarted) {
            if (IsPlayerGrounded()) {

                if (!isJumping) {
                    animator.SetBool("IsAirborne", false);
                    animator.SetBool("IsGliding", false);
                    rb.gravityScale = defaultGrav;
                    if (Input.GetButton("Jump")) {
                        jumpTimeCounter = jumpTime;
                        isJumping = true;
                    }
                }
                
            } else {

                animator.SetBool("IsAirborne", true);
                if (rb.velocity.y < 1 && Input.GetButton("Jump")) {
                    animator.SetBool("IsGliding", true);
                    rb.gravityScale = glidingGrav;
                } else {
                    animator.SetBool("IsGliding", false);
                    rb.gravityScale = defaultGrav;
                }
            }

        } else {

            if (Input.GetButtonUp("Jump")) {
                StartGame();
            }
        }
    }

    private void FixedUpdate() {
        if (isJumping) {
            if (Input.GetButton("Jump") && jumpTimeCounter > 0) {
                rb.velocity = Vector2.up * jumpForce;
                jumpTimeCounter -= Time.deltaTime;
            } else {
                jumpTimeCounter = 0;
                isJumping = false;
            }
        }
    }

    private bool IsPlayerGrounded() {
        return transform.Find("GroundCheck").GetComponent<GroundCheck>().GetIsGrounded();
    }

    void StartGame() {
        animator.SetBool("GameStarted", true);
        readyText.SetActive(false);
        gameController.GetComponent<GameController>().enabled = true;
        gameStarted = true;
    }
}