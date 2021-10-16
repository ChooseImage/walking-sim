/* 
   Ability System for a first person player controller, including
   1. Blink(Teleport) towards aiming spot in a range. Blink Ability needs charging and cooling down.
   2. Float from the ground. Float Ability needs cooling down and has a time limits in the air. 
   3. Move like skating with a higher speed and inertia. Press and hold W for skating; Release W for slowing down, Press and hold S for slowing down more quickly; Skating (W) also has a cool down.
*/
// Ver Alpha.
// Last modified on 10/13/2021
// By Roy Yang

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityStandardAssets.Characters.FirstPerson
{
    public class AbilitySystem : PublicVars
    {
        // Notice that SkateMode has grivity disabled while BlinkMode and FloatMode has it enabled.
        public bool Blink;
        public bool Float;
        public bool Skate;
        RaycastHit raycastHit;

        [Header("Blink Parameters")]
        [SerializeField] float BlinkRange = 30;
        [SerializeField] int SetBlinkTimes = 2;
        [SerializeField] float SetBlinkRechargingTime = 3;
        int BlinkTimes;

        [Header("Float Parameters")]
        bool FloatBegins = true;
        Vector3 FloatPosition;
        Vector3 FloatingPosition;
        [SerializeField] float SetFloatTime = 3;
        [SerializeField] float SetFloatCD = 6;
        float FloatTime;
        bool FloatIsReady;
        bool Floating;


        GameObject BlinkDestination()
        {
            Vector3 from = Camera.main.transform.position;
            Vector3 towards = Camera.main.transform.forward;
            if (Physics.Raycast(from, towards, out raycastHit, BlinkRange)) //LayserMask could be added as the fifith parameter for raycast hitting only the specific layer
                return raycastHit.collider.gameObject;
            else
                return null;
        }

        void StartBlink()
        {
            Rigidbody rb = gameObject.GetComponent<Rigidbody>();
            rb.useGravity = true;
            if (Input.GetKeyDown(KeyCode.Q) && BlinkTimes > 0)
            {
                if (BlinkDestination() != null)
                {
                    BlinkTimes--;
                    transform.position = raycastHit.point + raycastHit.normal * 1.5f; //Prevent from stucking in the hit objects
                }
                StartCoroutine(BlinkRecharge());
            }
        }

        IEnumerator BlinkRecharge()
        {
            yield return new WaitForSeconds(SetBlinkRechargingTime);
            if (BlinkTimes < SetBlinkTimes)
            {
                BlinkTimes++;
                yield break;
            }
        }

        bool InFloatingMode;
        void StartFloat()
        {
            Rigidbody rb = gameObject.GetComponent<Rigidbody>();
            rb.useGravity = true;

            // Boolean Variables InFloatingMode and Floating are both used because GroundCheck function will return True if player is a litle bit higher than the ground.
            if (PlayerIsGrounded && !InFloatingMode)
            {
                FloatTime = SetFloatTime;
                FloatPosition = transform.position + new Vector3(0, 2, 0);
                Floating = false;
            }
            if (Input.GetKey(KeyCode.E) && FloatTime > 0 && (FloatIsReady || Floating))
            {
                Floating = true;
                InFloatingMode = true;
                FloatTime -= Time.deltaTime;
                if (FloatBegins)
                {
                    FloatIsReady = false;
                    StartCoroutine(FloatInCD());
                    StartCoroutine(FloatAnime());
                    StartCoroutine(IncreaseFOV(Camera.main.fieldOfView));
                    rb.velocity = new Vector3(0, 0, 0);
                    FloatBegins = false;
                }
                rb.useGravity = false;
                PlayerFloatTime = FloatTime;
            }
            else
            {
                StartCoroutine(DecreaseFOV(Camera.main.fieldOfView));
                FloatBegins = true;
                InFloatingMode = false;
            }
        }

        IEnumerator FloatInCD()
        {
            yield return new WaitForSecondsRealtime(SetFloatCD);
            FloatIsReady = true;
            yield break;
        }

        IEnumerator FloatAnime()
        {
            float t = 0;
            while (transform.position.y < FloatPosition.y)
            {
                t += Time.deltaTime*.1f;
                float addY = .1f-Mathf.Lerp(0, .09f, t);
                transform.position += new Vector3(0, addY, 0);
                yield return null;
            }
            FloatPosition = transform.position;
            yield return StartCoroutine(FloatingAnime());
        }

        IEnumerator FloatingAnime()
        {
            float t = 0;
            while (Input.GetKey(KeyCode.E) && !Input.GetKey(KeyCode.Q) && FloatTime > 0)
            {
                t += Time.deltaTime;
                float addY = Mathf.PingPong(t*.5f, .5f);
                FloatingPosition = new Vector3(0, addY, 0) + FloatPosition;
                transform.position = FloatingPosition;
                yield return null;
            }
        }

        IEnumerator IncreaseFOV(float currentFOV)
        {
            float t = 0;
            while (Camera.main.fieldOfView < 70 && Input.GetKey(KeyCode.E) && FloatTime > 0)
            {
                Camera.main.fieldOfView = Mathf.Lerp(currentFOV, 70, t);
                t += Time.deltaTime * 1.2f;
                yield return null;
            }
        }

        //BUG!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! Short Press results in shaking!
        IEnumerator DecreaseFOV(float currentFOV)
        {
            float t = 0;
            while (Camera.main.fieldOfView > 60 && !(Input.GetKey(KeyCode.E) && FloatTime > 0))
            {
                Camera.main.fieldOfView -= .1f;
                t += Time.deltaTime * 1.2f;
                yield return null;
            }
        }

        bool SkateBegins;
        bool WUnlock;
        Vector3 SkatingForwardV;

        void StartSkate()
        {
            Rigidbody rb = gameObject.GetComponent<Rigidbody>();
            rb.useGravity = false;
            if (Input.GetKey(KeyCode.W) && WUnlock)
            {
                if (SkateBegins)
                {
                    SkateBegins = false;
                    rb.velocity = transform.forward * 2;
                }
                rb.mass = .5f;
                rb.AddForce(transform.forward * 5, ForceMode.Impulse);
                SkatingForwardV = rb.velocity;
            }
            else
            {
                rb.mass = 10;
            }
            if (Input.GetKeyUp(KeyCode.W))
            {
                SkateBegins = true;
                WUnlock = false;
                StartCoroutine(Inertial(rb));
                StartCoroutine(WLock());
            }
        }

        IEnumerator Inertial(Rigidbody rb)
        {
            float t = 0;
            while (t < 1)
            {
                t += Time.deltaTime * .5f;
                rb.velocity = new Vector3(Mathf.Lerp(SkatingForwardV.x, 0, t), Mathf.Lerp(SkatingForwardV.y, 0, t), Mathf.Lerp(SkatingForwardV.z, 0, t));
                float vx = rb.velocity.x;
                float vz = rb.velocity.z;
                float de = 5;
                if (Input.GetKey(KeyCode.S) && Mathf.Abs(vx) > de)
                {
                    vx += vx > 0 ? -de : de;
                    rb.velocity = new Vector3(vx, 0, vz);
                }
                if (Input.GetKey(KeyCode.S) && Mathf.Abs(vz) > de)
                {
                    vz += vx > 0 ? -de : de;
                    rb.velocity = new Vector3(vx, 0, vz);
                }
                yield return null;
            }
        }

        IEnumerator WLock()
        {
            yield return new WaitForSecondsRealtime(2f);
            WUnlock = true;
            yield break;
        }

        void Start()
        {
            BlinkTimes = SetBlinkTimes;
            FloatTime = SetFloatTime;
            FloatIsReady = true;
            Floating = false;
            SkateBegins = true;
            SkatingForwardV = new Vector3(0, 0, 0);
            WUnlock = true;
        }

        void Update()
        {
            if (Blink)
            {
                EnableJump = true;
                gameObject.GetComponent<Rigidbody>().mass = 10;
                StartBlink();
                PlayerBlinkTimes = BlinkTimes;
            }

            // Below are moved to the FixedUpdate for solving the bug. Doesn't work.;-(
            //if (Float)
            //{
            //    EnableJump = true;
            //    gameObject.GetComponent<Rigidbody>().mass = 10;
            //    PlayerFloatIsReady = FloatIsReady;
            //    PlayerFloatTime = FloatTime;
            //    StartFloat();
            //}

            if (Skate)
            {
                EnableJump = false;
                StartSkate();
            }      
        }

        private void FixedUpdate()
        {
            if (Float)
            {
                EnableJump = true;
                gameObject.GetComponent<Rigidbody>().mass = 10;
                PlayerFloatIsReady = FloatIsReady;
                PlayerFloatTime = FloatTime;
                StartFloat();
            }
        }
    }
}