using System.Collections;
using Unity.Netcode;
using UnityEngine;

namespace James.Script
{
    public enum EnemyState
    {
        Inactive,
        Phase1,
        Phase2,
        Phase3,
    }

    public class BossDummy : BossHealth, IEnemy
    {
        [Header("Boss Bullet")]
        [SerializeField] private GameObject bulletObject;
        [SerializeField] private float bulletSpeed = 10f;
        [SerializeField] private float firingSpeed = 3f;
        [SerializeField] [Tooltip("Normally starts at 0")] private int bulletCount;
        private bool alternatePattern = false;
        private bool isShoot;
        private EnemyState state;
        private Transform attackTarget;

        private void FixedUpdate()
        {
            EnemyLogic();
        }

        private void EnemyLogic()
        {
            #region Various state effects
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
                case EnemyState.Phase3:
                    firingSpeed = 1.75f;
                    StartCoroutine(ShootMultipleDirections());
                    break;
            }
            #endregion

            #region Conditions to change stats
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
                else if (bulletCount < 32)
                {
                    state = EnemyState.Phase2;
                }
                else
                {
                    state = EnemyState.Phase3;
                }
            }
            #endregion
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
                ShootBulletServerRpc(direction);
                bulletCount++; // +1 bullet count;
            }

            yield return new WaitForSeconds(firingSpeed);
            isShoot = true;
        }

        private IEnumerator ShootMultipleDirections()
        {
            isShoot = false;
            float[] angles = alternatePattern ? new float[] { 22.5f, 67.5f, 112.5f, 157.5f, 202.5f, 247.5f, 292.5f, 337.5f }
                                              : new float[] { 0f, 45f, 90f, 135f, 180f, 225f, 270f, 315f };
            foreach (float angle in angles)
            {
                Vector2 direction = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
                ShootBulletServerRpc(direction);
            }

            alternatePattern = !alternatePattern; // Toggle pattern
            bulletCount += angles.Length; // +7 bullet count;

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

        [ServerRpc(RequireOwnership = false)]
        private void ShootBulletServerRpc(Vector2 direction)
        {
            SoundManager.Instance.Play(SoundManager.SoundName.LaserGun);
            GameObject bulletInstance = Instantiate(bulletObject, transform.position, Quaternion.identity);

            // Rotate bullet to direction
            bulletInstance.transform.up = direction;
            bulletInstance.GetComponent<Rigidbody2D>().velocity = direction * bulletSpeed;
            
            ShootBulletClientRpc(direction);
        }

        [ClientRpc(RequireOwnership = false)]
        private void ShootBulletClientRpc(Vector2 direction)
        {
            if (IsOwner)
            {
                return;
            }
            GameObject bulletInstance = Instantiate(bulletObject, transform.position, Quaternion.identity);

            // Rotate bullet to direction
            bulletInstance.transform.up = direction;
            bulletInstance.GetComponent<Rigidbody2D>().velocity = direction * bulletSpeed;
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
