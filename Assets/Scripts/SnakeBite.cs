using UnityEngine;

public class SnakeBite : MonoBehaviour
{
    public float biteX;

    private bool isBiting = false;
    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.x <= biteX && !isBiting)
        {
            animator.SetBool("PlayerNear", true);
            isBiting = true;
        }
    }
}