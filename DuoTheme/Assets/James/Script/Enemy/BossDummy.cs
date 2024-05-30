using System.Collections;
using UnityEngine;

namespace James.Script
{
    public enum EnemyState
    {
        Inactive,
        Phase1,
        Phase2,
    }

    public class BossDummy : BossHealth, IEnemy
    {
        [Header("Boss Bullet")]
        [SerializeField] private GameObject bulletObject;
        [SerializeField] private float bulletSpeed = 10f;
        [SerializeField] private float firingSpeed = 3f;
        [SerializeField] [Tooltip("Normally starts at 0")] private int bulletCount;
        private bool isShoot;
        private Transform attackTarget;
        private EnemyState state;

        private void FixedUpdate()
        {
            EnemyLogic();
        }

        private void EnemyLogic()
        {
            // Various state effects
            switch (state)
            {
                case EnemyState.Inactive:
                    break;
                case EnemyState.Phase1:
                    firingSpeed = (bulletCount == 8) ? 0.1f : (bulletCount == 16) ? 3f : firingSpeed;
                    StartCoroutine(Shoot());
                    break;
                case EnemyState.Phase2:
                    firingSpeed = 1.5f;
                    StartCoroutine(Shoot());
                    break;
            }

            // Conditions to change stats
            if (!isShoot)
            {
                state = EnemyState.Inactive;
            }
            else if (isShoot)
            {
                if (bulletCount < 24)
                {
                    state = EnemyState.Phase1;
                }
                else
                {
                    state = EnemyState.Phase2;
                }
            }
        }

        #region Find and Attack target
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
                bulletCount++; // +1 bullet count;

                // Rotate bullet to direction
                bulletInstance.transform.up = direction;
                bulletInstance.GetComponent<Rigidbody2D>().velocity = direction * bulletSpeed;
            }

            yield return new WaitForSeconds(firingSpeed);
            isShoot = true;
        }

        private Transform FindTarget()
        {
            Collider2D[] players = Physics2D.OverlapCircleAll(transform.position, AreaBossRadius.x, PlayerLayer);
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
        #endregion

        // We called this function in BossHealth.cs
        #region Start & Stop shooting
        public void StartShooting()
        {
            isShoot = true;
        }

        public void StopShooting()
        {
            isShoot = false;
        }
        #endregion

        public void OnDie()
        {

        }
    }
}
