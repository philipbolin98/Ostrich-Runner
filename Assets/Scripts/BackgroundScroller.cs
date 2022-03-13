using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScroller : MonoBehaviour {
    public float scrollSpeed;
    private Renderer bgRenderer;

    // Start is called before the first frame update
    void Start() {
        bgRenderer = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update() {
        bgRenderer.material.mainTextureOffset += new Vector2((scrollSpeed/100) * Time.deltaTime, 0f);
    }
}