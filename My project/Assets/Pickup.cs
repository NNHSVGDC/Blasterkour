using UnityEngine;

public class Pickup : MonoBehaviour
{
    public float pickupRange = 10f;
    public float moveForce = 250f;
    public Transform holdParent; // The parent transform where picked objects should be held.

    private GameObject heldObj;

    void Update()
    {
        if(Input.GetButtonDown("Fire1")) // Assuming Fire1 is the left mouse button by default.
        {
            if(heldObj == null)
            {
                // Try to pick up the object.
                RaycastHit hit;
                if(Physics.Raycast(transform.position, transform.forward, out hit, pickupRange))
                {
                    if(hit.collider.gameObject.CompareTag("Pickupable"))
                    {
                        PickObject(hit.collider.gameObject);
                    }
                }
            }
            else
            {
                // Place the object.
                PlaceObject();
            }
        }

        if(heldObj != null)
        {
            MoveObject();
        }
    }

    void PickObject(GameObject pickObj)
    {
        if(pickObj.GetComponent<Rigidbody>())
        {
            Rigidbody objRb = pickObj.GetComponent<Rigidbody>();
            objRb.useGravity = false;
            objRb.drag = 10;

            objRb.transform.parent = holdParent;
            heldObj = pickObj;
        }
    }

    void PlaceObject()
    {
        if(heldObj.GetComponent<Rigidbody>())
        {
            Rigidbody objRb = heldObj.GetComponent<Rigidbody>();
            objRb.useGravity = true;
            objRb.drag = 1;

            heldObj.transform.parent = null;
            heldObj = null;
        }
    }

    void MoveObject()
    {
        if(Vector3.Distance(heldObj.transform.position, holdParent.position) > 0.1f)
        {
            Vector3 moveDirection = (holdParent.position - heldObj.transform.position);
            heldObj.GetComponent<Rigidbody>().AddForce(moveDirection * moveForce);
        }
    }
}