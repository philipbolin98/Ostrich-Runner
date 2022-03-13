using UnityEngine;

public class PerchedVulture : MonoBehaviour {
    private Transform playerTransform;
    private Animator animator;
    private bool canFly = false;
    private bool isFlying = false;
    private int flyX;

    public GameObject flyingVulture;
    public Vector2Int flyRange;
    public float flyPercent;

    // Start is called before the first frame update
    void Start() {
        flyX = Random.Range(flyRange.x, flyRange.y + 1);
        animator = GetComponent<Animator>();
        if (GameObject.FindWithTag("Player")) {
            playerTransform = GameObject.FindWithTag("Player").transform;
        }
        canFly = Random.value < (flyPercent / 100);
    }

    // Update is called once per frame
    void Update() {
        if (transform.position.x <= flyX && canFly && !isFlying) {
            isFlying = true;
            Fly();
        } else if(playerTransform && transform.position.x < playerTransform.position.x && !isFlying) {
            animator.SetBool("lookRight", true);
        }
    }

    public void Fly() {
        GameObject vulture = Instantiate(flyingVulture, new Vector2(flyX, transform.position.y + 1), Quaternion.identity);
        vulture.GetComponent<Vulture>().divePercent = 0;

        animator.SetBool("isFlying", true);
        GetComponent<CapsuleCollider2D>().enabled = false;
    }
}