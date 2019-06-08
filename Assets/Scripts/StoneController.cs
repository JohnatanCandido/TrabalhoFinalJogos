using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneController : MonoBehaviour {

    public float velocidade;
    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        transform.position = new Vector3(transform.position.x + velocidade, transform.position.y, transform.position.z);
    }
}
