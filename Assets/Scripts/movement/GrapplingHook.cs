using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

namespace TwinHookController
{
    interface Grapple
    {
        public void Grapple();
    }
    public class GrapplingHook : MonoBehaviour
    {

        [SerializeField] private Player pm;
        [SerializeField] private Collider grappleSphere;
        [SerializeField] private float grappleRange;
        [SerializeField] private LayerMask grappleLayer;
        [SerializeField] private LineRenderer lineRenderer;
        [SerializeField] private Transform guntip;



        private float inputDelay = 2f;
        private float inputDelayTimer = 0f;
        private bool inRange = false;
        private bool grappling = false;
        private List<Transform> grapplePoints = new List<Transform>();
        private Transform closest = null;
        private float grappleDelayTime;

        [SerializeField] private float grappleCooldown;
        [SerializeField] private float overshootY;
        private float grappleCooldownTimer;


      


        //TODO:
        //prioritize the closest thing in range




        // Start is called before the first frame update
        void Start()
        {
            pm = GetComponent<Player>();
            Debug.Log(pm.grapple);
        }
        void Update()
        {
            StartCoroutine(HandleInputs());


            if (grappleCooldownTimer > 0)
            {
                grappleCooldownTimer -= Time.deltaTime; // count down
            }

        }

        private void LateUpdate()
        {
            if (grappling)
            {
                lineRenderer.SetPosition(0, guntip.position);
            }
        }

        void OnTriggerEnter(Collider other)
        {
            inRange = true;


            if (other.gameObject.layer == LayerMask.NameToLayer("hookable"))
            {
                Debug.Log("Entered range of anchor: " + other.name);
                Transform anchor = other.transform;
                if (!grapplePoints.Contains(anchor))
                {
                    grapplePoints.Add(anchor);
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            inRange = false;
            if (other.gameObject.layer == LayerMask.NameToLayer("hookable"))
            {
                Debug.Log("Exited range of anchor: " + other.name);
                Transform anchor = other.transform;
                grapplePoints.Remove(anchor);
            }
        
        }
        // Update is called once per frame

        void startGrapple()
        {
            if (grappleCooldownTimer > 0 || grapplePoints.Count == 0) return; // return if we just grappled
                                                                              // Clean out null (destroyed) anchors
            grapplePoints.RemoveAll(p => p == null);

            // Find closest anchor
            float closestDist = Mathf.Infinity;

            foreach (Transform point in grapplePoints)
            {
                float dist = Vector3.Distance(transform.position, point.position);
                if (dist < closestDist)
                {
                    closestDist = dist;
                    closest = point;
                }
            }

            if (closest != null)
            {
                grappling = true;
                lineRenderer.enabled = true; // turn on line
                lineRenderer.SetPosition(1, closest.position); //endpoint of line
                pm.grapplePoint = closest;

                Invoke(nameof(executeGrapple), grappleDelayTime); //invoke for delayTime
            }
            else // if there is nothing to grapple
            {
                Invoke(nameof(stopGrapple), grappleDelayTime); //invoke for delayTime
            }
        }


        void executeGrapple()
        {
            //pm.isFrozen = false;
            float displacementY = closest.position.y - transform.position.y;
            float trajectoryHeight = displacementY > 0 ? displacementY + overshootY : overshootY;


            pm.jumpToPosition(closest.position, trajectoryHeight);


            Invoke(nameof(stopGrapple), 1f); //change this if we dont want to stop by itself
        }

        void stopGrapple()
        {
            pm.activeGrapple = false;
            grappling = false;
            grappleCooldownTimer = grappleCooldown;
            lineRenderer.enabled = false;
        }

        public void ForceStopGrapple()
        {
            stopGrapple();
        }



        IEnumerator HandleInputs()
        {
            if (inputDelayTimer < inputDelay)
            {
                if (grappling && Input.GetButtonDown(pm.grapple) && inRange)
                {
                    Debug.Log(pm.grapple);
                    stopGrapple();  // if grappling and pressing again, stop grappling
                    inputDelayTimer = 0;
                    Debug.Log("let go");
                    yield break;
                }
                if (grappling && !inRange) stopGrapple(); //if not in range & still grappling
                if (!grappling && Input.GetButtonDown(pm.grapple) && inRange)
                {
                    startGrapple(); // grapple if: not grappling, pressing button and in range
                    inputDelayTimer = 0;
                    Debug.Log("grapple");
                    yield break;
                }
            }
            else
            {
                inputDelayTimer += Time.deltaTime;
            }

            yield return null;
        }



    }

}

