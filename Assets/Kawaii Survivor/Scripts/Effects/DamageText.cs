using UnityEngine;
using TMPro;

public class DamageText : MonoBehaviour
{
    [Header("Element")]
    [SerializeField] private Animator animator;
    [SerializeField] private TextMeshPro damageText;
    public void Animate(int damage, bool isCriticalHit)
    {
        damageText.text = damage.ToString();
        damageText.color = isCriticalHit ? Color.yellow : Color.white;
        animator.Play("Animate");
    }
}
