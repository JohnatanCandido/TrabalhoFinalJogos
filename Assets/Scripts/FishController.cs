using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishController : MonoBehaviour {

    private Rigidbody2D rb;
    public AudioClip somPulo;
    private AudioSource audioSrc;
    public GameObject player;

    public float jumpTime;

    public float lastJump;

    public float jumpForce;


    // Start is called before the first frame update
    void Start() {
        rb = GetComponent<Rigidbody2D>();
        audioSrc = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update() {
        if (Time.time - lastJump >= jumpTime) {
            Vector2 forca = new Vector2(0, jumpForce);
            rb.AddForce(forca);
            lastJump = Time.time;
            if (Math.Abs(player.transform.position.x - transform.position.x) < 11F) {
                audioSrc.PlayOneShot(somPulo);
            }
        }
        if (transform.position.y > 4.5F) {
            float escalaY = Math.Abs(transform.localScale.y) * -1;
            transform.localScale = new Vector3(transform.localScale.x, escalaY, transform.localScale.z);
        } else if (transform.position.y < -2F) {
            float escalaY = Math.Abs(transform.localScale.y);
            transform.localScale = new Vector3(transform.localScale.x, escalaY, transform.localScale.z);
        }
    }
        
}
