using System.Collections;
using com.cyborgAssets.inspectorButtonPro;
using UnityEngine;
using DG.Tweening; // Не забудьте подключить DoTween

public class Tree : MonoBehaviour, IDamageable
{
    public ResourcesType resourcesType;
    public GameObject[] treeParts; // Массив кусков дерева
    public float bounceForce = 1f; // Сила баунса (отскока)
    public float respawnTimeMin = 5f; // Минимальное время восстановления
    public float respawnTimeMax = 10f; // Максимальное время восстановления
    public float scaleDownDuration = 0.2f; // Длительность уменьшения размера
    public float scaleUpDuration = 0.5f; // Длительность восстановления размера
    public int maxHealth = 8; // Максимальное количество жизней

    public int countObjAdd = 2;
    public Collider colliderCapsule;
    private int currentPartIndex; // Текущий индекс куска дерева
    private int currentHealth; // Текущее количество жизней
    private bool isTreeDestroyed = false; // Флаг, что дерево полностью уничтожено
    private float healthPerPart; // Количество HP для отрубания одной части
    private PlayerAttack playerAttack;
    private RecourcesInfo ObjectData;
    private DataManager dataManager;

    private void Start()
    {
        dataManager = DataManager.Instance;
        ObjectData = dataManager.GetDataResources(resourcesType);
        ResetTree();
    }

    [ProButton]
    public void ChopTree()
    {
        if (isTreeDestroyed) return;

        // Отскок дерева через DoTween


        // Уменьшаем текущий кусок дерева до нуля
        if (currentPartIndex < treeParts.Length)
        {
            treeParts[currentPartIndex].SetActive(false);

            currentPartIndex++;
            MoveText newMoveText = Instantiate(dataManager.moveText, transform.position + new Vector3(0, 3, 0),
                Quaternion.identity);
            newMoveText.Spawn(countObjAdd, ObjectData.icon);

            for (int i = 0; i < countObjAdd; i++)
            {
                FlyingObject newFlyingObject =
                    Instantiate(dataManager.flyingObject, transform.position, Quaternion.identity);
                newFlyingObject.JumpAndRotate(countObjAdd, playerAttack, ObjectData.model);
            }


            // Если все части дерева уменьшены, начинаем таймер на восстановление
            if (currentPartIndex >= treeParts.Length)
            {
                isTreeDestroyed = true;
                colliderCapsule.enabled = false;
                StartCoroutine(RespawnTree());
            }
        }
    }

    private void BounceTree()
    {
        // Анимация баунса (увеличение и уменьшение масштаба)
        transform.DOScale(Vector3.one * 0.9f, bounceForce).OnComplete(() =>
        {
            transform.DOScale(Vector3.one, bounceForce);
        });
    }

    private IEnumerator RespawnTree()
    {
        // Ожидание перед восстановлением дерева
        float respawnTime = Random.Range(respawnTimeMin, respawnTimeMax);
        yield return new WaitForSeconds(respawnTime);
        colliderCapsule.enabled = true;
        ResetTree();
    }

    private void ResetTree()
    {
        currentPartIndex = 0;
        isTreeDestroyed = false;
        currentHealth = maxHealth;
        healthPerPart = (float)maxHealth / treeParts.Length;
        // Восстанавливаем размеры всех частей дерева через DoTween
        foreach (GameObject part in treeParts)
        {
            part.transform.gameObject.SetActive(true);
            part.transform.DOScale(Vector3.one, scaleUpDuration);
        }
    }

    public void TakeDamage(int amount, PlayerAttack player)
    {
        if (playerAttack == null)
        {
            playerAttack = player;
        }

        if (currentHealth <= 0) return;

        // Уменьшаем количество жизней
        currentHealth -= amount;
        BounceTree();
        // Проверяем, нужно ли отрубить часть дерева
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            for (int i = 0; i < treeParts.Length; i++)
            {
                ChopTree();
            }

            isTreeDestroyed = true;
            StartCoroutine(RespawnTree());
        }
        else
        {
            int requiredPartsToChop = Mathf.FloorToInt((maxHealth - currentHealth) / healthPerPart);


            if (requiredPartsToChop > currentPartIndex)
            {
                int count = requiredPartsToChop - currentPartIndex;
                for (int i = 0; i < count; i++)
                {
                    ChopTree();
                }
            }
        }
    }
}