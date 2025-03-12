using System.Collections;
using UnityEngine;

public abstract class DroppableCurrency : MonoBehaviour,ICollectable
{
    private bool collected;

    private void OnEnable()
    {
        collected = false;
    }

    public void Collect(Player player)
    {
        if (collected)
            return;

        collected = true;

        StartCoroutine(MoveTowardsPlayer(player));
    }

    IEnumerator MoveTowardsPlayer(Player player)
    {
        float timer = 0;
        Vector2 initialPositon = transform.position;

        while (timer < 1)
        {
            Vector2 targetPosition = player.ShootedCenter();
            transform.position = Vector2.Lerp(initialPositon, targetPosition, timer);
            timer += Time.deltaTime;
            yield return null;
        }

        Collected();
    }

    protected abstract void Collected();
}
