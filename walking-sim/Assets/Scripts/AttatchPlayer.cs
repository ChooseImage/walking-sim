using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttatchPlayer : MonoBehaviour
{

    public GameObject Player;

    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("player") || other.CompareTag("Clock")){
            Player.transform.parent = transform;
        }
    }

    private void OnTriggerExit(Collider other) {
                if(other.CompareTag("player") || other.CompareTag("Clock")){
            Player.transform.parent = null;
        }
    }
}
