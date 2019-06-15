using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour {

    private int hp;

    public float speed;

    // Start is called before the first frame update
    void Start() {
        hp = 5;
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetMouseButton(0)) {
            print("a");
            Vector3 target = Camera.main.ScreenToWorldPoint(Input.mousePosition);//converting
            target.z = transform.position.z;
            transform.position = Vector3.MoveTowards(transform.position, target, speed);
        }
    }

    private void OnCollisionEnter2D(Collision2D other) {
        Vector3 contactPoint = other.contacts[0].point;
        Vector3 center = GetComponent<Collider>().bounds.center;
 
        if (other.collider.tag == "Player" && contactPoint.y > center.y) {
            hp--;
            if (hp == 0) {
                Destroy(gameObject);
            }
        }
    }

    
}
