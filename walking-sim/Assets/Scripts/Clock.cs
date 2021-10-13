using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clock : MonoBehaviour
{
   

    public Light lt;

    void Start()
    {
       lt = GetComponent<Light>(); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other){
        // if(other.CompareTag("block")){
        //     lt.color = Color.red;
        // }
    }
}
