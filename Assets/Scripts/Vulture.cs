using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vulture : MonoBehaviour {

    private Rigidbody2D rb;
    private Animator animator;

    public float horizontalSpeed;
    public float diveX;
    public float verticalSpeed;
    public float divePercent;

    private bool canDive;
    private bool isDiving = false;

    // Start is called before the first frame update
    void Start() {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = Vector2.left * horizontalSpeed;

        animator = GetComponent<Animator>();

        canDive = Random.value < (divePercent / 100);
    }

    // Update is called once per frame
    void Update() {

        if (canDive && !isDiving && transform.position.x < diveX) {
            isDiving = true;
            Dive();
        }
    }

    public void Dive() {

        GameObject player = GameObject.FindWithTag("Player");

        if (player) {

            //float angle = Vector3.Angle(transform.position, player.transform.position) - 45;

            animator.SetBool("Dive", true);
            rb.velocity = new Vector2(rb.velocity.x + 1, -1 * verticalSpeed);

            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 45);

            CapsuleCollider2D collider = GetComponent<CapsuleCollider2D>();
            collider.offset = new Vector2(collider.offset.x, -.05f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.name != "PlatformSpawnTrigger") {
            Destroy(gameObject);
        }
    }
}