using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    #region Variables
    public float speed;
    public GameObject muzzlePrefab;    // 발사 이펙트
    public GameObject hitPrefab;

    public AudioClip shotSFX;
    public AudioClip hitSFX;

    bool collided;
    Rigidbody rigid;

    [HideInInspector]
    public AttackBehaviour attackBehaviour;

    [HideInInspector]
    public GameObject owner;
    [HideInInspector]
    public GameObject target;
    #endregion Variables


    protected virtual void Start() 
    {
        if (target != null) {
            Vector3 dest = target.transform.position;
            dest.y += 1.5f;     // 발 끝이 기준이라 살짝 올림
            transform.LookAt(dest);
        }

        if (owner) {
            Collider projectileCollider = GetComponent<Collider>();
            Collider[] ownerColliders = owner.GetComponentsInChildren<Collider>();

            foreach (Collider col in ownerColliders) {
                Physics.IgnoreCollision(projectileCollider, col);
            }
        }

        rigid = GetComponent<Rigidbody>();

        if (muzzlePrefab != null) {
            GameObject muzzleVFX = Instantiate(muzzlePrefab, transform.position, Quaternion.identity);
            muzzleVFX.transform.forward = gameObject.transform.forward;

            ParticleSystem particleSystem = muzzleVFX.GetComponent<ParticleSystem>();
            if (particleSystem) {
                Destroy(muzzleVFX, particleSystem.main.duration);
            } else {
                ParticleSystem childParticleSystem = muzzleVFX.transform.GetChild(0).GetComponent<ParticleSystem>();
                if (childParticleSystem) {
                    Destroy(muzzleVFX, childParticleSystem.main.duration);
                }
            }
        }

        if (shotSFX != null && GetComponent<AudioSource>()) {
            GetComponent<AudioSource>().PlayOneShot(shotSFX);
        }
    }

    protected virtual void FixedUpdate()  // rigidbody를 사용하기 때문에
    {
        if (speed != 0 && rigid != null) {
            rigid.position += (transform.forward) * (speed * Time.deltaTime);
        }
    }

    void OnCollisionEnter(Collision other) {
        if (collided)   return;
        collided = true;

        Collider projectileCollider = GetComponent<Collider>();
        projectileCollider.enabled = false;

        if (hitSFX != null && GetComponent<AudioSource>()) {
            GetComponent<AudioSource>().PlayOneShot(hitSFX);
        }

        speed = 0;
        rigid.isKinematic = true;   // 더이상 물리연산 하지 않음 -> CollisionEnter 호출 하지 않음

        // 맞은 위치 계산
        ContactPoint contact = other.contacts[0];
        Quaternion contactRotation = Quaternion.FromToRotation(Vector3.up, contact.normal);
        Vector3 contactPos = contact.point;

        if (hitPrefab) {
            GameObject hitVFX = Instantiate(hitPrefab, contactPos, contactRotation);

            ParticleSystem particleSystem = hitVFX.GetComponent<ParticleSystem>();
            if (particleSystem) {
                Destroy(hitVFX, particleSystem.main.duration);
            } else {
                ParticleSystem childParticleSystem = hitVFX.transform.GetChild(0).GetComponent<ParticleSystem>();
                if (childParticleSystem) {
                    Destroy(hitVFX, childParticleSystem.main.duration);
                }
            }
        }

        IDamageable damageable = other.gameObject.GetComponent<IDamageable>();
        if (damageable != null) {
            damageable.TakeDamage(attackBehaviour?.damage ?? 0, null);
        }

        StartCoroutine(DestroyParticle(0f));
    }

    public IEnumerator DestroyParticle(float waitTime) {
        if (transform.childCount > 0 && waitTime != 0) {
            List<Transform> childs = new List<Transform>();

            foreach (Transform t in transform.GetChild(0).transform) {
                childs.Add(t);
            }

            while (transform.GetChild(0).localScale.x > 0) {
                yield return new WaitForSeconds(0.01f);

                transform.GetChild(0).localScale -= new Vector3(0.1f, 0.1f, 0.1f);
                for (int i = 0; i < childs.Count; i++) {
                    childs[i].localScale -= new Vector3(0.1f, 0.1f, 0.1f);
                }
            }
        }

        yield return new WaitForSeconds(waitTime);
        Destroy(gameObject);
    }
}
