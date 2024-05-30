using System.Collections;
using UnityEngine;

namespace James.Script
{
    public class BossDummy : BossHealth, IEnemy
    {
        [SerializeField] private GameObject bulletObject;
        [SerializeField] private float bulletSpeed;
        [SerializeField] private float firingSpeed;

        private bool isShoot;
        private Transform attackTarget;

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

        private IEnumerator Shoot()
        {
            isShoot = false;

            // Find target
            attackTarget = FindTarget();
            if (attackTarget != null)
            {
                // Shoot to direction
                Vector2 direction = (attackTarget.position - transform.position).normalized;
                GameObject bulletInstance = Instantiate(bulletObject, transform.position, Quaternion.identity);

                // Rotate bullet to direction
                bulletInstance.transform.up = direction;
                bulletInstance.GetComponent<Rigidbody2D>().velocity = direction * bulletSpeed;
            }

            yield return new WaitForSeconds(firingSpeed);
            isShoot = true;
        }

        private Transform FindTarget()
        {
            Collider2D[] players = Physics2D.OverlapCircleAll(transform.position, areaBossRadius.x, playerLayer);
            Transform nearestPlayer = null;
            float shortestDistance = Mathf.Infinity;

            foreach (Collider2D player in players)
            {
                float distance = Vector2.Distance(transform.position, player.transform.position);
                if (distance < shortestDistance)
                {
                    shortestDistance = distance;
                    nearestPlayer = player.transform;
                }
            }
            return nearestPlayer;
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

        }
    }
}
