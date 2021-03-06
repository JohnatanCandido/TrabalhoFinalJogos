﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneController : MonoBehaviour {

    private Rigidbody2D rb;
    public GameObject player;

    public float velocidade;

    public float rotacao;

    // Start is called before the first frame update
    void Start() {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update() {
        if (player.transform.position.x > -15F) {
            transform.position = new Vector3(transform.position.x + (velocidade * Time.deltaTime), transform.position.y, transform.position.z);
            rb.rotation -= rotacao;
        }
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.collider.tag == "Barreira") {
            velocidade = 0F;
            rotacao = 0F;
        }
    }
}
