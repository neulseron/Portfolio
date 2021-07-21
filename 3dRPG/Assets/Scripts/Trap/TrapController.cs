using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapController : MonoBehaviour
{
    public float damageInterval = 0.5f;     // 몇초마다 데미지
    public float damageDuration = 5f;
    public int damage = 5;

    float calcDuration = 0f;

    [SerializeField]
    //ParticleSystem effect;

    IDamageable damageable;


    private void Update() {
        if (damageable != null) {
            calcDuration -= Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider other) {
        damageable = other.GetComponent<IDamageable>();
        if (damageable != null) {
            calcDuration = damageDuration;

            //effect?.Play();
            StartCoroutine(ProcessDamage());
        }
    }

    private void OnTriggerExit(Collider other) {
        damageable = null;
        StopAllCoroutines();
        //effect?.Stop();
    }

    IEnumerator ProcessDamage()
    {
        while (calcDuration > 0 && damageable != null) {
            damageable.TakeDamage(damage, null);

            yield return new WaitForSeconds(damageInterval);
        }

        damageable = null;
        //effect?.Stop();
    }
}
