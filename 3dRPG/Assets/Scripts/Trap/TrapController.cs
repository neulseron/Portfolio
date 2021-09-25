using System.Collections;
using UnityEngine;

public class TrapController : MonoBehaviour
{
#region Variables
    public float damageInterval = 0.5f;     // 몇초마다 데미지
    public float damageDuration = 5f;
    public int damage = 5;

    float calcDuration = 0f;

    IDamageable damageable;
#endregion Variables


#region Methods
    private void Update() {
        if (damageable != null) {
            calcDuration -= Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider other) {
        damageable = other.GetComponent<IDamageable>();
        if (damageable != null) {
            calcDuration = damageDuration;

            StartCoroutine(ProcessDamage());
        }
    }

    private void OnTriggerExit(Collider other) {
        damageable = null;
        StopAllCoroutines();
    }

    IEnumerator ProcessDamage()
    {
        while (calcDuration > 0 && damageable != null) {
            damageable.TakeDamage(damage, null);

            yield return new WaitForSeconds(damageInterval);
        }

        damageable = null;
    }
#endregion Methods
}
