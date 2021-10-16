using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public Light lt;

    void Start()
    {
        lt = GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.R)){
            SceneManager.LoadScene("SampleScene");
        }
         lt.color -= (Color.white / 2.0f) * Time.deltaTime;
    }

    void OnTriggerEnter(Collider other){


        if(other.CompareTag("Clock") & PublicVars.collectables >=1){
        //directionalLight.color = Color.red;
        print("change color");
        }

        if(other.CompareTag("Collectable")){
            PublicVars.collectables += 1;
            //aud.PlayOneShot(blipSound);
            print("collectables: "+ PublicVars.collectables);
            Destroy(other.gameObject);
            
        }
  }
}
