using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

    public Animator anim;

    private float posInicial;

    public float velocidade;

    public float tempoAnim;

    private float lastAnim;

    public bool attack;

    public GameObject player;

    private float timeOfDeath;

    public float deathPoint;

    // Start is called before the first frame update
    void Start() {
        anim = GetComponent<Animator>();
        posInicial = transform.position.x;
        lastAnim = Time.time;
        anim.SetBool("Andando", false);
    }

    // Update is called once per frame
    void Update() {
        checaMorte();
        if (!anim.GetBool("Morreu")) {
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
    }

    private void checaAtacar() {

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
}
