using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour {

    public int hp;

    public float speed;

    private List<Vector3> positions;

    private int currPos;

    public float posTime;

    private float lastPosTime;

    private int lastPosIndex;

    private Animator anim;

    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start() {
        currPos = -1;
        lastPosIndex = -1;
        initBossPositions();
        lastPosTime = Time.time;
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void initBossPositions() {
        positions = new List<Vector3>();
        positions.Add(new Vector3(7F, 7.5F, transform.position.z));
        positions.Add(new Vector3(-7F, 7.5F, transform.position.z));
        positions.Add(new Vector3(7F, 1F, transform.position.z));
        positions.Add(new Vector3(-7F, 1F, transform.position.z));
        positions.Add(new Vector3(-1F, 6F, transform.position.z));
        changePosition();
    }

    // Update is called once per frame
    void Update() {
        if (!anim.GetBool("Morreu")) {
            transform.position = Vector3.MoveTowards(transform.position, positions[currPos], speed * Time.deltaTime);
            if (Time.time - lastPosTime > posTime) {
                changePosition();
            }
            checaDirecao();
        }

    }

    private void checaDirecao() {
        if (transform.position.x > positions[currPos].x) {
            transform.localScale = new Vector3(Math.Abs(transform.localScale.x) * -1, transform.localScale.y, transform.localScale.z);
        } else if (transform.position.x < positions[currPos].x) {
            transform.localScale = new Vector3(Math.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        } else if (positions[currPos].x > 0F) {
            transform.localScale = new Vector3(Math.Abs(transform.localScale.x) * -1, transform.localScale.y, transform.localScale.z);
        } else {
            transform.localScale = new Vector3(Math.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
    }

    private void changePosition() {
        System.Random rand = new System.Random();
        while (currPos == lastPosIndex) {
            currPos = rand.Next(5);
        }
        lastPosTime = Time.time;
        lastPosIndex = currPos;
    }

    private void OnCollisionEnter2D(Collision2D other) {
        Collider2D collider = other.collider;
        Vector3 contactPoint = other.contacts[0].point;
        Vector3 center = collider.bounds.center;

        if (contactPoint.y > 1.6F) {
            hp--;
            if (hp == 0) {
                anim.SetBool("Morreu", true);
                rb.gravityScale = 1;
            } else {
                changePosition();
            }
        }
    }
}
