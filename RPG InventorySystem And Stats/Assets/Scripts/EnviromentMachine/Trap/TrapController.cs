using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapController : MonoBehaviour
{
    public float damageInterval = 0.5f;     // 데미지를 받는 간격
    public float damageDuration = 5.0f;     // 데미지를 입히는 시간
    public int damage = 5;                  // 데미지

    float calcDuration = 0.0f;              // 계산에 사용할 변수

    [SerializeField]
    ParticleSystem effect;                  // 이펙트 파티클

    IDamageable damageable;                 // 데미지 주체

    private void Update()
    {
        // 주체가 있다면 지속시간 계산
        if (damageable != null)
        {
            calcDuration -= Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        damageable = other.GetComponentInParent<IDamageable>();
        if (damageable != null)
        {
            calcDuration = damageDuration;

            // 이펙트 플레이 및 데미지 프로세스 호출
            effect.Play();
            StartCoroutine(ProcessDamage());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // 초기화
        damageable = null;
        StopAllCoroutines();
        effect.Stop();
    }

    /// <summary>
    /// 데미지를 입히는 코루틴 함수
    /// </summary>
    /// <returns></returns>
    IEnumerator ProcessDamage()
    {
        // 지속시간이 남아있고 주체가 있다면 간격마다 데미지 처리
        while (calcDuration > 0 && damageable != null)
        {
            damageable.TakeDamage(damage, null);

            yield return new WaitForSeconds(damageInterval);
        }

        // 초기화
        damageable = null;
        effect.Stop();
    }
}
