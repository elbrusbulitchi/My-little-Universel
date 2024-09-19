using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public Vector2 attackRadius = new Vector2(5f, 3f);
    public Vector3 attackOffset = Vector3.zero;
    public int attackDamage = 10;
    public Animator animator;

    private bool isAttacking = false;

    private void Update()
    {
        CheckForEnemies();
    }

    private void CheckForEnemies()
    {
        if (isAttacking) return;

        Collider[] hitColliders = GetEnemiesInRange();
        
        foreach (var hitCollider in hitColliders)
        {
            IDamageable damageable = hitCollider.GetComponent<IDamageable>();
            if (damageable != null)
            {
                AutoAttack();
                return;
            }
        }
    }

    private void AutoAttack()
    {
        isAttacking = true;
        animator.SetTrigger("Attack");
    }

    public void Attack()
    {
        Collider[] hitColliders = GetEnemiesInRange();

        foreach (var hitCollider in hitColliders)
        {
            IDamageable damageable = hitCollider.GetComponent<IDamageable>();
            if (damageable != null)
            {
                damageable.TakeDamage(attackDamage, this);
            }
        }

        isAttacking = false;
    }

    private Collider[] GetEnemiesInRange()
    {
        Vector3 attackCenter = transform.TransformPoint(attackOffset);
        return Physics.OverlapBox(attackCenter, new Vector3(attackRadius.x, 1f, attackRadius.y));
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector3 attackCenter = transform.TransformPoint(attackOffset);
        Gizmos.DrawWireCube(attackCenter, new Vector3(attackRadius.x * 2, 1f, attackRadius.y * 2));
    }
}