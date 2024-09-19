using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class MoveText : MonoBehaviour
{
    [SerializeField] private TextMeshPro textMeshPro;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private float moveDuration = 1.5f; // Длительность движения
    [SerializeField] private float fadeDuration = 0.5f; // Длительность фейда
    [SerializeField] private float randomRangeX = 1f; // Диапазон случайного смещения по X
    [SerializeField] private float randomRangeY = 1f; 

    private Camera mainCamera;

    private void Awake()
    {
        mainCamera = Camera.main; // Получаем основную камеру
    }

    // Метод для спавна текста в определённой точке и запуска анимации
    public void Spawn(int text, Sprite sprite)
    {
        textMeshPro.text = "+" + text;

        SetAlpha(0);

        // Устанавливаем спрайт в зависимости от типа
        spriteRenderer.sprite = sprite;

        textMeshPro.DOFade(1f, fadeDuration); 
        spriteRenderer.DOFade(1f, fadeDuration); 

        Vector3 randomDirection = new Vector3(Random.Range(-randomRangeX, randomRangeX), Random.Range(1, randomRangeY), 0f);

        transform.DOMove(transform.position + randomDirection, moveDuration).SetEase(Ease.OutQuad);

        Sequence sequence = DOTween.Sequence();
        sequence.AppendInterval(moveDuration - fadeDuration); // Ждём до момента исчезновения
        sequence.Append(textMeshPro.DOFade(0f, fadeDuration)); // Исчезновение текста
        sequence.Join(spriteRenderer.DOFade(0f, fadeDuration)); // Исчезновение спрайта
        sequence.OnComplete(() => Destroy(gameObject)); // Уничтожаем объект после анимации
    }

    private void Update()
    {
        LookAtCamera();
    }

    private void LookAtCamera()
    {
        // Рассчитываем направление на камеру
        Vector3 directionToCamera = mainCamera.transform.position - transform.position;

        // Поворачиваем объект к камере
        transform.LookAt(transform.position + directionToCamera);
    }

    private void SetAlpha(float alpha)
    {
        Color textColor = textMeshPro.color;
        textColor.a = alpha;
        textMeshPro.color = textColor;

        Color spriteColor = spriteRenderer.color;
        spriteColor.a = alpha;
        spriteRenderer.color = spriteColor;
    }

   
}
