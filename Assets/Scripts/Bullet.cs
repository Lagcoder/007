using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 20f;
    public string bulletType = "Standard";

    public Transform target;
    public GameObject sender;
    private GameObject original;

    public void Initialize(Transform targetTransform, GameObject bulletSender, string type, GameObject originalSender)
    {
        target = targetTransform;
        original = originalSender;
        sender = bulletSender;
        bulletType = type;
    }

    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }
        Vector3 direction = (target.position - transform.position).normalized;
        float step = speed * Time.deltaTime;
        transform.position = Vector2.MoveTowards(transform.position, target.position, step);
        Debug.DrawRay(transform.position, direction * 5f, Color.red); 
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject hitObject = collision.gameObject;

        IBulletHittable hittable = hitObject.GetComponent<IBulletHittable>();
        if (hittable != null)
        {
            hittable.OnBulletHit(bulletType, sender, original);
            Destroy(gameObject);
        }

    }
}