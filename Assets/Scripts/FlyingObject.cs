using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

public class FlyingObject : MonoBehaviour
{
    [SerializeField] private float jumpPower = 5f; // Сила прыжка
    [SerializeField] private float moveDuration = 2f; // Длительность движения
    [SerializeField] private float rotationSpeed = 360f;

    private PlayerAttack target;
    private Vector3 randomDirection;

    public void JumpAndRotate(int countObjAdd, PlayerAttack playerAttack, GameObject model)
    {
        Instantiate(model, transform);
        this.target = playerAttack;
        randomDirection = new Vector3(Random.Range(-3f, 3f), 1f, Random.Range(-3f, 3f)).normalized;

        Vector3 jumpTarget = transform.position + randomDirection * Random.Range(1f, 3f);

        transform.DOJump(jumpTarget, jumpPower, 1, moveDuration / 2).OnComplete(() => { FlyToTarget(); });

        Vector3 randomRotation = new Vector3(Random.Range(-rotationSpeed, rotationSpeed),
            Random.Range(-rotationSpeed, rotationSpeed),
            Random.Range(-rotationSpeed, rotationSpeed));

        transform.DORotate(randomRotation, moveDuration / 2, RotateMode.FastBeyond360)
            .SetLoops(-1, LoopType.Restart);
    }

    private void FlyToTarget()
    {
        transform.DOKill(false);

        if (target != null)
        {
            transform.DOMove(target.transform.position + new Vector3(0, 0.5f, 0), 0.1f).SetEase(Ease.InQuad)
                .OnComplete(() => { Destroy(gameObject); });
        }
    }
}