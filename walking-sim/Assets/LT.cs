using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LT : MonoBehaviour
{

    public GameObject Block1;
    public GameObject Block2;
    public GameObject Block3;
    public GameObject Block4;
    public GameObject Block5;
    public GameObject Block6;
    public GameObject Block7;
    public GameObject Block8;
    public GameObject Block9;
   //public GameObject Block2;



    void Start()
    {
         LeanTween.moveY(Block1, 3, 3).setLoopPingPong();
         LeanTween.moveY(Block2, 7, 14).setLoopPingPong();
         LeanTween.moveY(Block3, 7, 8).setLoopPingPong();
         LeanTween.moveY(Block4, 7, 7).setLoopPingPong();
         LeanTween.moveY(Block5, 9, 4).setLoopPingPong();
         LeanTween.moveY(Block6, 8, 9).setLoopPingPong();
         LeanTween.moveY(Block7, 5, 13).setLoopPingPong();
         LeanTween.moveY(Block8, 7, 9).setLoopPingPong();
         LeanTween.moveY(Block9, 9, 13).setLoopPingPong();
    }

    // Update is called once per frame
    void Update()
    {
       
    }
}
