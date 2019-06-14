using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishController : MonoBehaviour {

    public float jumpTime;

    private float lastJump;

    public float jumpForce;

    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start() {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update() {
        if (Time.time - lastJump >= jumpTime) {
            Vector2 forca = new Vector2(0, jumpForce);
            rb.AddForce(forca);
            lastJump = Time.time;
        }
    }
        
}
