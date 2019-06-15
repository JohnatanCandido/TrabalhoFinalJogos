using System.Collections;
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

    private GameObject fish;

    // Start is called before the first frame update
    void Start() {
        pulando = false;
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
        if (!morreu) {
            checaPulo();
            checaMovimento();
        } else if (fish != null) {
            transform.position = new Vector3(fish.transform.position.x, fish.transform.position.y + 1, fish.transform.position.z);
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
        if (transform.position.y < 0F) {
            morreu = true;
            anim.SetBool("Esmagado", true);
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
            Vector2 offsetTextura = matFundo.mainTextureOffset;
            matFundo.mainTextureOffset = new Vector2(offsetTextura.x + addX * velocidadeFundo, offsetTextura.y);
        }
    }

    private void OnCollisionEnter2D(Collision2D other) {
        Collider2D collider = other.collider;
        Vector3 contactPoint = other.contacts[0].point;
        Vector3 center = collider.bounds.center;

        if(collider.tag == "Enemy") {
            colisaoEnemy(contactPoint, collider, center);
        } else if (collider.tag == "Boss") {
            colisaoBoss(contactPoint, center);
        }
    }

    private void colisaoEnemy(Vector3 contactPoint, Collider2D collider, Vector3 center) {
        if (contactPoint.y > center.y) {
                Destroy(collider.gameObject);
                Vector2 forca = new Vector2(0, forcaAplicar * 0.8F);
                rb.AddForce(forca);
            } else {
                morreu = true;
                anim.SetBool("Esmagado", true);
        }
    }

    private void colisaoFish(GameObject gameObject) {
        fish = gameObject;
        morreu = true;
        anim.SetBool("Esmagado", true);
    }

    private void colisaoBoss(Vector3 contactPoint, Vector3 center) {
        if (contactPoint.y > 1.6F) {
            Vector2 forca = new Vector2(0, forcaAplicar);
            rb.AddForce(forca);
        } else {
            morreu = true;
            anim.SetBool("Esmagado", true);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "Pedra" || other.gameObject.tag == "Trap") {
            morreu = true;
            anim.SetBool("Esmagado", true);
        } else if (other.gameObject.tag == "Fish") {
            colisaoFish(other.gameObject);
        }
    }
}
