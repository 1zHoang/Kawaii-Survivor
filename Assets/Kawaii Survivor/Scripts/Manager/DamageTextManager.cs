using UnityEngine;
using UnityEngine.Pool;

public class DamageTextManager : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private DamageText damageTextPrefeb;

    [Header("Pooling")]
    private ObjectPool<DamageText> damageTextPool;

    private void Awake()
    {
        Enemy.onDamageTaken += EnemyHitCallback;
    }
    private void OnDestroy()
    {
        Enemy.onDamageTaken -= EnemyHitCallback;
    }

    void Start()
    {
        damageTextPool = new ObjectPool<DamageText>(CreateFunction, ActionOnGet, ActionOnRelease, ActionOnDestroy);
    }

    private DamageText CreateFunction()
    {
        return Instantiate(damageTextPrefeb, transform);
    }

    //private void ActionOnGet(DamageText damageText)
    //{
    //    damageText.gameObject.SetActive(true);
    //}

    private void ActionOnGet(DamageText damageText)
    {
        Debug.Log("[POOL] Lấy DamageText từ pool");
        damageText.gameObject.SetActive(true);
        damageText.transform.localScale = Vector3.one; // Đảm bảo không bị scale về 0
    }

    private void ActionOnRelease(DamageText damageText)
    {
        Debug.Log("[POOL] Trả DamageText về pool");
        damageText.gameObject.SetActive(false);
    }


    //private void ActionOnRelease(DamageText damageText)
    //{
    //    if(damageText != null)
    //    {
    //        damageText.gameObject.SetActive(false);
    //    }
    //}

    private void ActionOnDestroy(DamageText damageText)
    {
        Destroy(damageText.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
       
    }
    private void EnemyHitCallback(int damage, Vector2 enemyPos, bool isCriticalHit)
    {
        Vector3 spawnPosition = enemyPos + Vector2.up * 1.5f;

        DamageText damageTextInstance = damageTextPool.Get();
        damageTextInstance.transform.position = spawnPosition;

        damageTextInstance.Animate(damage, isCriticalHit);

        LeanTween.delayedCall(1, () => damageTextPool.Release(damageTextInstance));
    }
}
