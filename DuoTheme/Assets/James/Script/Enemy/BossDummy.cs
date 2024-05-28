using System;
using System.Collections;
using UnityEngine;

namespace James.Script
{
    public class BossDummy : BossHealth, IEnemy
    {
        [SerializeField] private GameObject bulletObject;
        [SerializeField] private float bulletSpeed;
        private bool isShoot;
        

        private void FixedUpdate()
        {
            ShootBullet();
        }

        private void ShootBullet()
        {
            if (!isShoot)
            {
                StartCoroutine(Shoot());
            }
            
        }
        
        IEnumerator Shoot()
        {
            isShoot = true;
            GameObject bulletObject = Instantiate(this.bulletObject, transform.position, transform.rotation);
            bulletObject.GetComponent<Rigidbody2D>().velocity = bulletSpeed * transform.up;
            yield return new WaitForSeconds(4);
            isShoot = false;
        }

        public void OnDie()
        {
            UnActiveBoss();
        }
    }
}
