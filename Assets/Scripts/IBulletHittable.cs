using UnityEngine;
public interface IBulletHittable
{
    void OnBulletHit(string bulletType, GameObject sender, GameObject originalSender);
}
