using UnityEngine;

public class TargetSystem : MonoBehaviour 
{
    [Header("Detection Settings")]
    public float rayDistance = 8.0f;
    public float originOffset = 2.0f;
    public LayerMask targetLayer;

    // This makes it a true MonoBehaviour function you can call from elsewhere
    public GameObject getTarget()
    {
        // 1. Calculate the 'Forward' starting point in 2D
        Vector2 facingDir = transform.up; 
        Vector2 origin = (Vector2)transform.position - (facingDir * originOffset);

        // 2. Fire the Raycast
        RaycastHit2D hit = Physics2D.Raycast(origin, facingDir, rayDistance, targetLayer);

        // 3. Visualization for the Scene View
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

    // Optional: Only used if you want it running every frame automatically
    private void Update()
    {
        getTarget();
    }
}