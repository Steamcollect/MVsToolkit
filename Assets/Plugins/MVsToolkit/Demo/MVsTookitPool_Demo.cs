using MVsToolkit.Pool;
using MVsToolkit.Utils;
using UnityEngine;

public class MVsTookitPool_Demo : MonoBehaviour
{
    public Transform attackPoint;

    [Space(10)]
    public PoolObject<GameObject> bullets;

    private void Start()
    {
        bullets.Init();
    }

    public void Attack()
    {
        if (bullets.TryGet(out GameObject bullet))
        {
            bullet.transform.position = attackPoint.position;

            this.Delay(() =>
            {
                bullets.Release(bullet);
            }, 3);
        }
    }
}
