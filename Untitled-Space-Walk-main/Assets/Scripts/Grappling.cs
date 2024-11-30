using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grappling : MonoBehaviour
{
    [Header("References")]
    private PlayerMovement pm;
    public Transform cam;
    public Transform gunTip;
    public LayerMask whatIsGrappleable;
    public LineRenderer lr;

    [Header("Grappling Settings")]
    public float maxGrappleDistance = 50f; // Max distance for grappling
    public float grappleDelayTime = 0.3f; // Delay before grappling starts
    public float overshootYAxis = 2f; // How much the player overshoots vertically
    public float grappleForceMultiplier = 10f; // The base force applied to the player when grappling
    public float grappleForceDecay = 1f; // How much the force decays as you approach the grapple point

    private Vector3 grapplePoint;
    private Transform grappleTransform;

    [Header("Cooldown Settings")]
    public float grapplingCd = 3f; // Time before another grapple can be used
    private float grapplingCdTimer;

    [Header("Grappling Input")]
    public KeyCode grappleKey = KeyCode.Mouse0; // Default to mouse button 0 (left-click)
    private bool grappling;
    private bool isGrapplingActive;

    private Rigidbody rb;

    private void Start()
    {
        pm = GetComponent<PlayerMovement>();
        rb = GetComponent<Rigidbody>();  // Get the Rigidbody component
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && grapplingCdTimer <= 0)  
        {
            StartGrapple();
            Debug.Log("Grapple started");
        }

        if (Input.GetMouseButtonUp(0) && isGrapplingActive)
        {
            StopGrapple();
            Debug.Log("Grapple stopped");
        }

        // Countdown for the cooldown timer
        if (grapplingCdTimer > 0)
            grapplingCdTimer -= Time.deltaTime;
    }

    private void LateUpdate()
    {
        // Update the LineRenderer to visualize the grapple line
        if (grappling && grappleTransform != null)
        {
            lr.SetPosition(0, gunTip.position);
            lr.SetPosition(1, grappleTransform.position);
        }
    }

    private void StartGrapple()
    {
        if (grapplingCdTimer > 0) return;  // Prevent grappling during cooldown

        grappling = true;

        RaycastHit hit;
        if (Physics.Raycast(cam.position, cam.forward, out hit, maxGrappleDistance, whatIsGrappleable))
        {
            grapplePoint = hit.point;
        }
        else
        {
            grapplePoint = Vector3.zero;
        }

        // Create a new temporary transform at the grapple point
        if (grapplePoint != Vector3.zero)
        {
            grappleTransform = new GameObject("GrapplePoint").transform;
            grappleTransform.position = grapplePoint;

            // Enable the LineRenderer and set the target position
            lr.enabled = true;
            lr.SetPosition(1, grapplePoint);

            // Apply initial force to start grappling
            isGrapplingActive = true;
        }
    }

    private void FixedUpdate()
    {
        // Apply increasing force towards the grapple point if grappling
        if (isGrapplingActive && grappleTransform != null)
        {
            Vector3 directionToGrapplePoint = (grappleTransform.position - transform.position).normalized;
            float distanceToGrapplePoint = Vector3.Distance(transform.position, grappleTransform.position);

            // Apply force that increases with time and distance to the grapple point
            float forceMagnitude = Mathf.Clamp(distanceToGrapplePoint * grappleForceMultiplier, 0f, 50f);

            // Decay the force the closer we get to the grapple point (smooth stopping)
            forceMagnitude *= Mathf.Lerp(1f, 0f, distanceToGrapplePoint / maxGrappleDistance);
            
            rb.AddForce(directionToGrapplePoint * forceMagnitude, ForceMode.Impulse);
        }
    }

    private void StopGrapple()
    {
        grappling = false;
        isGrapplingActive = false;
        grapplingCdTimer = grapplingCd; // Start cooldown

        // Disable the LineRenderer
        lr.enabled = false;

        // Destroy the temporary grapple point transform
        if (grappleTransform != null)
        {
            Destroy(grappleTransform.gameObject);
        }
    }

    public bool IsGrappling()
    {
        return grappling;
    }

    public Vector3 GetGrapplePoint()
    {
        return grapplePoint;
    }

    // Use Gizmos to show the grapple point in the Scene view
    private void OnDrawGizmos()
    {
        if (grappling && grapplePoint != Vector3.zero)
        {
            Gizmos.color = Color.green;  // Color of the gizmo
            Gizmos.DrawSphere(grapplePoint, 0.2f);  // Draw a small sphere at the grapple point
        }
    }
}
