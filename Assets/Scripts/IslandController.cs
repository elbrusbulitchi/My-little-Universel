using System.Collections.Generic;
using UnityEngine;

public class IslandController : Singleton<IslandController>
{
    [SerializeField] private Island[] islands;     // Массив островов в сцене
    private IslandData islandData;

    private const string IslandsKey = "IslandsData"; // Ключ для хранения данных в PlayerPrefs

    private void Start()
    {
        // Получаем все острова в сцене
        islands = GetComponentsInChildren<Island>();

        // Загрузка данных островов
        LoadIslandsData();
    }

    private void LoadIslandsData()
    {
        // Если данных нет в PlayerPrefs, создаем их с начальными данными
        if (!PlayerPrefs.HasKey(IslandsKey))
        {
            CreateInitialIslandsData();
        }
        else
        {
            // Загружаем данные из PlayerPrefs
            string json = PlayerPrefs.GetString(IslandsKey);
            islandData = JsonUtility.FromJson<IslandData>(json);

            // Проверяем, если добавились новые острова
            if (islands.Length > islandData.islands.Count)
            {
                // Добавляем новые острова в данные
                for (int i = islandData.islands.Count; i < islands.Length; i++)
                {
                    islandData.islands.Add(new IslandInfo { indexIsland = i, isOpen = false });
                    islands[i].SetInfo(i, false); // По умолчанию закрываем новые острова
                }

                // Сохраняем обновленные данные
                SaveIslandsData();
            }

            // Применяем данные к островам
            for (int i = 0; i < islandData.islands.Count; i++)
            {
                bool isOpen = islandData.islands[i].isOpen;
                islands[i].SetInfo(i, isOpen);
            }
        }
    }

    private void CreateInitialIslandsData()
    {
        // Изначально открываем только первый остров
        islandData = new IslandData { islands = new List<IslandInfo>() };

        for (int i = 0; i < islands.Length; i++)
        {
            bool isOpen = false; 
            islands[i].SetInfo(i, isOpen);

            islandData.islands.Add(new IslandInfo { indexIsland = i, isOpen = isOpen });
        }

        // Сохраняем начальные данные в PlayerPrefs
        SaveIslandsData();
    }

    public void SaveIslandsData()
    {
        // Преобразуем данные в JSON
        string json = JsonUtility.ToJson(islandData, true);
        PlayerPrefs.SetString(IslandsKey, json);
        PlayerPrefs.Save();
    }

    // Метод для изменения состояния острова (например, когда игрок открывает новый остров)
    public void OpenIsland(int index)
    {
        if (index >= 0 && index < islands.Length)
        {
            islands[index].SetInfo(index, true);
            islandData.islands[index].isOpen = true;
            SaveIslandsData(); // Сохраняем изменения
        }
    }
}
