using System.Collections.Generic;
using KinematicCharacterController.Examples;
using TMPro;
using UnityEngine;

public class Island : MonoBehaviour
{
    [SerializeField] private ResourcesType resourcesType;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private TextMeshPro textMeshPro;
    [SerializeField] private GameObject[] openIslands;
    [SerializeField] private int index;
    [SerializeField] private bool isOpen;
    [SerializeField] private int maxCount;
    private int count;
    public void SetInfo(int index, bool isOpen)
    {
        this.index = index;
        this.isOpen = isOpen;
        
        if (openIslands != null && openIslands.Length > 0)
        {
            for (int i = 0; i < openIslands.Length; i++)
            {
                // Проверяем, что элемент массива не null
                if (openIslands[i] != null)
                {
                    openIslands[i].gameObject.SetActive(isOpen);
                }
                else
                {
                    Debug.LogWarning($"Element at index {i} in openIslands array is null.");
                }
            }
        }
        else
        {
            Debug.LogWarning("openIslands array is null or empty.");
        }

        gameObject.SetActive(!isOpen);
        textMeshPro.text = count + "/" + maxCount;
        spriteRenderer.sprite = DataManager.Instance.GetDataResources(resourcesType).icon;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerController playerController) && !isOpen)
        {
            isOpen = true;
            IslandController.Instance.OpenIsland(index);
        }
    }
}


[System.Serializable]
public class IslandInfo
{
    public int indexIsland;
    public bool isOpen;
}

[System.Serializable]
public class IslandData
{
    public List<IslandInfo> islands;
}