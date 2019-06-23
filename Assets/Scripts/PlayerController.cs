using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    private GameObject fish;

    public float finishLine;

    private bool ganhou;

    private float timeOfEndgame;

    // Start is called before the first frame update
    void Start() {
        timeOfEndgame = -1F;
        pulando = false;
        ganhou = false;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        if (renderer != null) {
            camera = Camera.main;
            matFundo = renderer.material;
        }
        morreu = false;
    }

    // Update is called once per frame
    void Update() {
        if (!morreu && !ganhou) {
            if (transform.position.x > finishLine) {
                ganhou = true;
                timeOfEndgame = Time.time;
            }
            checaPulo();
            checaMovimento();
        } else if (fish != null) {
            transform.position = new Vector3(fish.transform.position.x, fish.transform.position.y + 1, fish.transform.position.z);
        }

        if (timeOfEndgame > 0 && Time.time - timeOfEndgame > 2F) {
            if (morreu) {
               MainController.die();
            } else if (ganhou) {
                MainController.win();
            }
        }
    }

    private void checaPulo() {
        if (Input.GetButtonDown("Jump") && !pulando) {
            pular(1F);
        }
        pulando = !Physics2D.Linecast(transform.position, refChao.position, mascara);
        anim.SetBool("Pulando", pulando);
    }

    private void checaMovimento() {
        if (transform.position.y < 0F) {
            morrer();
            return;
        }
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

        trataCamera(novoX, addX);
    }

    private void trataCamera(float novoX, float addX) {
        if (matFundo != null) {
            Vector3 posCam = new Vector3(novoX, camera.transform.position.y, camera.transform.position.z);
            camera.transform.position = posCam;
            if (renderer.tag != "FundoFaseTrem") {
                Vector2 offsetTextura = matFundo.mainTextureOffset;
                matFundo.mainTextureOffset = new Vector2(offsetTextura.x + addX * velocidadeFundo, offsetTextura.y);
            } else {
                Vector2 offsetTextura = matFundo.mainTextureOffset;
                matFundo.mainTextureOffset = new Vector2(offsetTextura.x + ((0.09F - (addX / 200)) * velocidadeFundo), offsetTextura.y);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other) {
        Collider2D collider = other.collider;
        Vector3 contactPoint = other.contacts[0].point;
        Vector3 center = collider.bounds.center;

        if (collider.tag == "Enemy") {
            colisaoEnemy(contactPoint, collider, center);
        }
    }

    private void colisaoEnemy(Vector3 contactPoint, Collider2D collider, Vector3 center) {
        if (contactPoint.y > center.y) {
            pular(0.8F);
        } else {
            morrer();
        }
    }

    private void colisaoFish(GameObject gameObject) {
        fish = gameObject;
        morrer();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "Pedra" || other.gameObject.tag == "Trap") {
            morrer();
        } else if (other.gameObject.tag == "Fish") {
            colisaoFish(other.gameObject);
        } else if (other.gameObject.tag == "Spell") {
            Destroy(other.gameObject);
            if (!morreu) {
                morrer();
            }
        }
    }

    public void pular(float multiplier) {
        Vector2 forca = new Vector2(0, forcaAplicar * multiplier);
        rb.AddForce(forca);
    }

    public void morrer() {
        morreu = true;
        anim.SetBool("Esmagado", true);
        timeOfEndgame = Time.time;
    }
}
