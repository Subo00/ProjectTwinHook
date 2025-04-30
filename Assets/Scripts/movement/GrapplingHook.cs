using System.Collections.Generic;
using UnityEngine;

namespace TwinHookController
{
    public class GrapplingHook : MonoBehaviour
    {
        [SerializeField] private Player pm;
        [SerializeField] private float grappleRange = 10f;
        [SerializeField] private LayerMask grappleLayer;
        [SerializeField] private LineRenderer lineRenderer;
        [SerializeField] private Transform guntip;

        [SerializeField] private float grappleCooldown = 1f;

        private List<Transform> grapplePoints = new List<Transform>();
        private Transform closest = null;

        private bool grappling = false;
        private float grappleCooldownTimer = 0f;

        private void Start()
        {
            pm = GetComponent<Player>();
        }

        private void Update()
        {
            if (!grappling) ScanForAnchors();
            HandleInputs();

            if (grappleCooldownTimer > 0)
                grappleCooldownTimer -= Time.deltaTime;

            if (grappling)
            {
                lineRenderer.SetPosition(0, guntip.position);
                lineRenderer.SetPosition(1, closest.position);


            }
        }

        private void ScanForAnchors()
        {
            grapplePoints.Clear();
            Collider[] hits = Physics.OverlapSphere(transform.position, grappleRange, grappleLayer);

            float closestDist = Mathf.Infinity;
            closest = null;

            foreach (var hit in hits)
            {
                Transform anchor = hit.transform;
                grapplePoints.Add(anchor);

                float dist = Vector3.Distance(transform.position, anchor.position);
                if (dist < closestDist)
                {
                    closest = anchor;
                    closestDist = dist;
                }
            }
        }

        private void HandleInputs()
        {
            if (Input.GetButtonDown(pm.grapple) && grappleCooldownTimer <= 0)
            {
                if (!grappling && closest != null)
                {
                    startGrapple();
                }
                else if (grappling)
                {
                    stopGrapple();
                }
            }
        }

        private void startGrapple()
        {
            grappling = true;
            lineRenderer.enabled = true;
            lineRenderer.SetPosition(1, closest.position);
            pm.grapplePoint = closest;
            pm.isFrozen = true;
            Invoke(nameof(executeGrapple), pm.stats.grappleDelay);
        }

        private void executeGrapple()
        {
            pm.isFrozen = false;    
            if (closest == null) return;

            float displacementY = closest.position.y - transform.position.y;
            float trajectoryHeight = displacementY > 0 ? displacementY + pm.stats.overshootY : pm.stats.overshootY;

            pm.jumpToPosition(closest.position, trajectoryHeight);

            Invoke(nameof(stopGrapple), 1f);
        }

        private void stopGrapple()
        {
            grappling = false;
            pm.activeGrapple = false;
            grappleCooldownTimer = pm.stats.grappleCooldown;
            lineRenderer.enabled = false;
        }

        public void ForceStopGrapple()
        {
            stopGrapple();
        }

        private void LateUpdate()
        {
            if (grappling)
            {
                lineRenderer.SetPosition(0, guntip.position);
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, grappleRange);
        }
    }
}
