using System.Collections.Generic;

public static class BulletRegistry
{
    private static readonly HashSet<Bullet> _activeBullets = new HashSet<Bullet>();

    public static int ActiveCount => _activeBullets.Count;

    public static void Register(Bullet bullet)
    {
        if (bullet != null)
            _activeBullets.Add(bullet);
    }

    public static void Unregister(Bullet bullet)
    {
        _activeBullets.Remove(bullet);
    }
}
