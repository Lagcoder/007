using UnityEngine;

public class TargetSystem : MonoBehaviour 
{
    [Header("Detection Settings")]
    public float rayDistance = 8.0f;
    public float originOffset = 2.0f;
    public LayerMask targetLayer;


    public GameObject getTarget()
    {
        Vector2 facingDir = transform.up; 
        Vector2 origin = (Vector2)transform.position + (facingDir * originOffset);
        RaycastHit2D hit = Physics2D.Raycast(origin, facingDir, rayDistance, targetLayer);
        if (hit.collider != null)
        {
            Debug.Log($"<color=cyan>getTarget hit:</color> {hit.collider.name} i am {gameObject.name}");
            Debug.DrawLine(origin, hit.point, Color.red);
            return hit.collider.gameObject; // Return the object we found
        }
        else
        {
            Debug.DrawRay(origin, facingDir * rayDistance, Color.green);
             Debug.Log($"<color=cyan>getTarget hit:</color> nothing");
            return null; // Hit nothing
        }
    }

    // debugging: fire raycast every frame to visualize it in the editor
    private void Update()
    {
        getTarget();
    }
}