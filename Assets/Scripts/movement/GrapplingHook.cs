using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


interface Grapple
{
    public void Grapple();
}
public class GrapplingHook : MonoBehaviour
{

 

    [SerializeField] private float grapplRange;
    [SerializeField] private LayerMask grappleLayer;

    public Transform grapplePoint;
    private DistanceJoint2D joint;
    // Start is called before the first frame update
    void Start()
    {
        joint = gameObject.GetComponent<DistanceJoint2D>();
        joint.enabled = false;
    }

    // Update is called once per frame

    void Update()
    {
        

        //doing the grappling promt here
        if(Input.GetKeyDown(KeyCode.E))
        {
            Ray r = new Ray(grapplePoint.position, grapplePoint.forward);
            if(Physics.Raycast(r, out RaycastHit hitInfo, grapplRange))
            {
                if(hitInfo.collider.gameObject.TryGetComponent(out Grapple grappableObj))
                {
                    grappableObj.Grapple();
                }
            }
        }






        //if(Input.GetMouseButtonDown(0))
        //{
        //    RaycastHit2D hit = Physics2D.Raycast(
        //        origin: Camera.main.ScreenToWorldPoint(Input.mousePosition),
        //        direction: Vector3.zero,
        //        distance: Mathf.Infinity,
        //        layerMask: grappleLayer
        //    );

        //    if(hit.collider != null )
        //    {
        //        grapplePoint = hit.point;
        //        grapplePoint.z = 0f;
        //        joint.connectedAnchor = grapplePoint;
        //        joint.enabled = true;
        //        joint.distance = grappleLength;
        //    }
        //}

        //if (Input.GetMouseButtonUp(0))
        //{
        //    joint.enabled = false;
        //}
    }
}
