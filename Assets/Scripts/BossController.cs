using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour {

    public AudioClip somSpell;
    private AudioSource audioSrc;
    private List<Vector3> positions;
    private Animator anim;
    private Rigidbody2D rb;
    public GameObject player;
    public GameObject objAttack;
    public Transform refAttack;

    public int hp;

    public float speed;

    private int currPos;

    public float posTime;

    private float lastPosTime;

    private int lastPosIndex;

    private float lastAttack;

    private float atkTime;

    // Start is called before the first frame update
    void Start() {
        currPos = -1;
        lastPosIndex = -1;
        initBossPositions();
        lastPosTime = Time.time;
        lastAttack = Time.time;
        atkTime = 2F;
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        audioSrc = GetComponent<AudioSource>();
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
        if (!anim.GetBool("Morreu") && !player.GetComponent<PlayerController>().morreu) {
            transform.position = Vector3.MoveTowards(transform.position, positions[currPos], speed * Time.deltaTime);
            if (Time.time - lastPosTime > posTime) {
                changePosition();
            }
            checaDirecao();
            if (deveAtirar()) {
                atirar();
            }
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

        if (contactPoint.y < center.y) {
            try {
                other.gameObject.GetComponent<PlayerController>().pular(1.5F);
                if (other.gameObject.GetComponent<PlayerController>().morreu) {
                    return;
                }
                hp--;
                print("HP: " + hp);
                if (hp == 0) {
                    anim.SetBool("Morreu", true);
                    rb.gravityScale = 1;
                } else {
                    rb.gravityScale = 0;
                    changePosition();
                }
            } catch {}
        } else {
            other.gameObject.GetComponent<PlayerController>().morrer();
        }
    }

    private bool deveAtirar() {
        return transform.position == positions[currPos] && Time.time - lastAttack > atkTime;
    }

    private void atirar() {
        lastAttack = Time.time;
        System.Random rand = new System.Random();
        if (rand.Next(4) < 3) {
            tiro1();
        } else {
            tiro2();
        }
    }

    private void tiro1() {
        geraTiro(player.transform.position);
    }

    private void tiro2() {
        System.Random rand = new System.Random();
        float espacamento = rand.Next(4) + 2F;
        Vector3 pos1 = new Vector3(player.transform.position.x + espacamento, player.transform.position.y + espacamento, player.transform.position.z);
        geraTiro(pos1);

        Vector3 pos2 = new Vector3(player.transform.position.x - espacamento, player.transform.position.y - espacamento, player.transform.position.z);
        geraTiro(pos2);
    }

    private void geraTiro(Vector3 position) {
        GameObject objNovo = Instantiate(objAttack, refAttack.position, refAttack.rotation);
        var heading = position - transform.position;
        var distance = heading.magnitude;
        var dir = heading / distance;
        if (hp > 3) {
            objNovo.GetComponent<BossSpellController>().setSpeed(dir.x, dir.y, 4F);
        } else {
            objNovo.GetComponent<BossSpellController>().setSpeed(dir.x, dir.y, 8F);
        }
        audioSrc.PlayOneShot(somSpell);
    }
}
