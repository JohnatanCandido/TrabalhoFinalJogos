using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

    private Animator anim;

    private float posInicial;

    public float velocidade;

    public float tempoAnim;

    private float lastAnim;

    // Start is called before the first frame update
    void Start() {
        anim = GetComponent<Animator>();
        posInicial = transform.position.x;
        lastAnim = Time.time;
        anim.SetBool("Andando", false);
    }

    // Update is called once per frame
    void Update() {
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
