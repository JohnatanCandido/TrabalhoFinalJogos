using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

    private Animator anim;
    public GameObject player;
    public GameObject objAttack;
    public Transform refAttack;
    public AudioClip somSpell;
    private AudioSource audioSrc;

    private float posInicial;

    public float velocidade;

    public float tempoAnim;

    private float lastAnim;

    public bool attack;

    private float lastAttack;

    private float timeOfDeath;

    public float deathPoint;

    private bool geraAttack;

    // Start is called before the first frame update
    void Start() {
        posInicial = transform.position.x;
        lastAnim = Time.time;
        lastAttack = 0F;
        anim = GetComponent<Animator>();
        audioSrc = GetComponent<AudioSource>();
        anim.SetBool("Andando", false);
    }

    // Update is called once per frame
    void Update() {
        checaMorte();
        if (!anim.GetBool("Morreu")) {
            if (attack) {
                checaAtacar();
            } else {
                andar();
            }
        }
    }

    private void andar() {
        if (anim.GetBool("Andando") && Time.time - lastAnim <= tempoAnim) {
            float novoX = transform.position.x + (velocidade * Time.deltaTime);
            transform.position = new Vector3(novoX, transform.position.y, transform.position.z);
        } else if (Time.time - lastAnim > tempoAnim) {
            anim.SetBool("Andando", !anim.GetBool("Andando"));
            lastAnim = Time.time;
            if (anim.GetBool("Andando")) {
                float escalaX = -transform.localScale.x;
                velocidade = -velocidade;
                transform.localScale = new Vector3(escalaX, transform.localScale.y, transform.localScale.z);
            }
        }
    }

    private void checaMorte() {
        if (anim.GetBool("Morreu") && Time.time - timeOfDeath > 1.5F) {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.collider.tag == "Player") {
            Vector3 contactPoint = other.contacts[0].point;
            if (contactPoint.y > deathPoint) {
                anim.SetBool("Morreu", true);
                timeOfDeath = Time.time;
            }
        }
    }

    private void checaAtacar() {
        mudaDirecao();
        if (Math.Abs(player.transform.position.x - transform.position.x) < 11F && Time.time - lastAttack > 2F) {
            geraAttack = true;
            anim.SetBool("Atacar", true);
            lastAttack = Time.time;
        } else if (Time.time - lastAttack > 1F) {
            anim.SetBool("Atacar", false);
        } else if (geraAttack && Time.time - lastAttack > 0.5F) {
            geraAttack = false;
            GameObject objNovo = Instantiate(objAttack, refAttack.position, refAttack.rotation);
            if (transform.localScale.x < 0) {
                objNovo.GetComponent<EnemyFireController>().setDirecao(-1);
            }
            audioSrc.PlayOneShot(somSpell);
        }
    }

    private void mudaDirecao() {
        float escalaX = Math.Abs(transform.localScale.x);
        if (player.transform.position.x < transform.position.x) {
            escalaX = -Math.Abs(transform.localScale.x);
        }
        transform.localScale = new Vector3(escalaX, transform.localScale.y, transform.localScale.z);
    }
}
