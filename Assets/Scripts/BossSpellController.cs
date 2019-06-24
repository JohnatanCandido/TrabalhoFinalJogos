using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpellController : MonoBehaviour {

    private float speedX;
    private float speedY;

    private float speed;

    public int direcao = +1;
    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start() {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update() {
        rb.rotation += 10F * direcao;
        float addX = speedX * Time.deltaTime;
        float novoX = transform.position.x + addX;
        float addY = speedY * Time.deltaTime;
        float novoY = transform.position.y + addY;
        transform.position = new Vector3(novoX, novoY, transform.position.z); 
    }

    public void setSpeed(float x, float y, float speed) {
        this.speedX = x * speed;
        this.speedY = y * speed;
    }
}
