using System.Collections;
using UnityEngine;

namespace James.Script
{
    public class BossDummy : BossHealth, IEnemy
    {
        [SerializeField] private GameObject bulletObject;
        [SerializeField] private float bulletSpeed;
        [SerializeField] private bool isShoot;

        private void FixedUpdate()
        {
            if (isShoot)
            {
                ShootBullet();
            }
        }

        private void ShootBullet()
        {
            StartCoroutine(Shoot());
        }

        IEnumerator Shoot()
        {
            isShoot = false;
            GameObject bulletInstance = Instantiate(bulletObject, transform.position, transform.rotation);
            bulletInstance.GetComponent<Rigidbody2D>().velocity = bulletSpeed * transform.up;
            yield return new WaitForSeconds(4);
            isShoot = true;
        }

        public void StartShooting()
        {
            isShoot = true;
        }

        public void StopShooting()
        {
            isShoot = false;
        }

        public void OnDie()
        {
            InactiveBoss();
        }
    }
}
