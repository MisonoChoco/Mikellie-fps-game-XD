using System;
using UnityEngine;
using UnityEngine.VFX;

public class Bullet : MonoBehaviour
{
    public int bulletDamage;
    public VisualEffect explosionEffect;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Target"))
        {
            print("hit " + collision.gameObject.name + " !");
            CreateBulletImpactEffect(collision);
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }

        if (collision.gameObject.CompareTag("Wall"))
        {
            print("hit wall!");
            CreateBulletImpactEffect(collision);
            Destroy(gameObject);
        }

        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (collision.gameObject.GetComponent<Enemy>().isDead == false)
            {
                collision.gameObject.GetComponent<Enemy>().TakeDamage(bulletDamage);
            }
            if (collision.gameObject.GetComponent<Enemy>().isDead == true)
            {
                collision.gameObject.GetComponent<CapsuleCollider>().enabled = false;
            }
            collision.gameObject.GetComponent<Enemy>().TakeDamage(bulletDamage);
            CreateBloodSprayFX(collision);
            Destroy(gameObject);
        }
    }

    private void CreateBloodSprayFX(Collision objectHit)
    {
        ContactPoint contact = objectHit.contacts[0];

        GameObject bloodSprayPrefab = Instantiate(GlobalReference.Instance.BloodSprayEffect, contact.point, Quaternion.LookRotation(contact.normal));

        bloodSprayPrefab.transform.SetParent(objectHit.gameObject.transform);
    }

    private void CreateBulletImpactEffect(Collision objectHit)
    {
        ContactPoint contact = objectHit.contacts[0];

        GameObject hole = Instantiate(GlobalReference.Instance.bulletImpactEffectPrefab, contact.point, Quaternion.LookRotation(contact.normal));

        hole.transform.SetParent(objectHit.gameObject.transform);
    }
}