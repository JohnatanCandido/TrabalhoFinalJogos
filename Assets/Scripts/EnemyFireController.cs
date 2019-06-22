using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFireController : MonoBehaviour {

    public float velocidade;
    public int direcao = 1;

    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start() {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update() {
        float addX = velocidade * direcao * Time.deltaTime;
        float novoX = transform.position.x + addX;
        transform.position = new Vector3(novoX, transform.position.y, transform.position.z);
        rb.rotation += 10F * direcao;
    }

    public void setDirecao(int dir) {
        this.direcao = dir;
    }

    void OnBecameInvisible() {
        Destroy(gameObject);
    }
}
