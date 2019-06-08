﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    private Rigidbody2D rb;

    public float forcaAplicar;

    public float velocidade;

    private float ladoAnterior = 1F;

    public Transform refChao;

    public LayerMask mascara;

    private Animator anim;

    private bool pulando;

    private Camera camera;

    private Material matFundo;

    public Renderer renderer;

    public float velocidadeFundo;

    public bool morreu;

    // Start is called before the first frame update
    void Start() {
        pulando = false;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        camera = Camera.main;
        matFundo = renderer.material;
        morreu = false;
    }

    // Update is called once per frame
    void Update() {
        if (!morreu) {
            checaPulo();
            checaMovimento();
        }
    }

    private void checaPulo() {
        if (Input.GetButtonDown("Jump") && !pulando) {
            Vector2 forca = new Vector2(0, forcaAplicar);
            rb.AddForce(forca);
        }
        pulando = !Physics2D.Linecast(transform.position, refChao.position, mascara);
        anim.SetBool("Pulando", pulando);
    }

    private void checaMovimento() {
        float deslocX = Input.GetAxisRaw("Horizontal");
        float addX = Input.GetAxisRaw("Horizontal") * velocidade * Time.deltaTime;
        float novoX = transform.position.x + addX;
        transform.position = new Vector3(novoX, transform.position.y, transform.position.z);
        
        if (ladoAnterior != deslocX && deslocX != 0) {
            ladoAnterior = deslocX;
            float escalaX = transform.localScale.x * -1;
            transform.localScale = new Vector3(escalaX, transform.localScale.y, transform.localScale.z);
        }
        anim.SetBool("Correndo", deslocX != 0 && !pulando);

        Vector3 posCam = new Vector3(novoX, camera.transform.position.y, camera.transform.position.z);
        camera.transform.position = posCam;

        Vector2 teste = matFundo.mainTextureOffset;
        matFundo.mainTextureOffset = new Vector2(teste.x + addX * velocidadeFundo, teste.y);
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "Pedra") {
            morreu = true;
            anim.SetBool("Esmagado", true);
        }
    }
}
