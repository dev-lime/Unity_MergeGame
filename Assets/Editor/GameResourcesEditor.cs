using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

[CustomEditor(typeof(GameResources))]
public class GameResourcesEditor : Editor
{
    private int subListSize = 4; // Размер вложенных списков (по умолчанию 4)
    private Dictionary<int, bool> foldoutStates = new Dictionary<int, bool>(); // Состояние сворачивания для каждого списка

    public override void OnInspectorGUI()
    {
        // Получаем ссылку на объект GameResources
        GameResources gameResources = (GameResources)target;

        // Используем subListSize из GameResources
        gameResources.subListSize = EditorGUILayout.IntField("Sub List Size", gameResources.subListSize);
        gameResources.subListSize = Mathf.Max(1, gameResources.subListSize); // Минимум 1

        // Отображаем каждый вложенный список
        for (int i = 0; i < gameResources.items.Count; i++)
        {
            // Инициализация состояния сворачивания для текущего списка
            if (!foldoutStates.ContainsKey(i))
                foldoutStates[i] = false; // По умолчанию список свёрнут

            // Отображаем заголовок списка с кнопкой сворачивания
            foldoutStates[i] = EditorGUILayout.Foldout(foldoutStates[i], $"Item List {i}", true);

            // Если список развёрнут, отображаем его элементы
            if (foldoutStates[i])
            {
                EditorGUI.indentLevel++; // Увеличиваем отступ для вложенных элементов

                // Убедимся, что размер вложенного списка соответствует subListSize
                ResizeSubList(gameResources.items[i], gameResources.subListSize);

                // Отображаем поля для каждого элемента вложенного списка
                for (int j = 0; j < gameResources.items[i].Count; j++)
                {
                    gameResources.items[i][j] = (Sprite)EditorGUILayout.ObjectField(
                        $"Element {j}",
                        gameResources.items[i][j],
                        typeof(Sprite),
                        false
                    );
                }

                // Кнопка для удаления текущего списка
                if (GUILayout.Button("Remove This List"))
                {
                    gameResources.items.RemoveAt(i);
                    foldoutStates.Remove(i); // Удаляем состояние сворачивания для удалённого списка
                    i--; // Корректируем индекс после удаления
                    SaveGameResources(gameResources); // Сохраняем изменения
                    break; // Выходим из цикла, чтобы избежать ошибок
                }

                EditorGUI.indentLevel--; // Уменьшаем отступ
            }

            EditorGUILayout.Space(); // Добавляем отступ между списками
        }

        EditorGUILayout.Separator();
        // Кнопка для добавления нового списка
        if (GUILayout.Button(new GUIContent(" Add New List", EditorGUIUtility.IconContent("d_Toolbar Plus").image)))
        {
            AddNewList(gameResources);
        }

        // Сохраняем изменения, если они были
        if (GUI.changed)
        {
            SaveGameResources(gameResources);
        }
    }

    // Метод для добавления нового списка
    private void AddNewList(GameResources gameResources)
    {
        List<Sprite> newList = new List<Sprite>();
        for (int i = 0; i < subListSize; i++)
        {
            newList.Add(null); // Инициализируем каждый элемент значением null
        }
        gameResources.items.Add(newList);
        SaveGameResources(gameResources); // Сохраняем изменения
    }

    // Метод для изменения размера вложенного списка
    private void ResizeSubList(List<Sprite> subList, int targetSize)
    {
        while (subList.Count < targetSize)
        {
            subList.Add(null); // Добавляем элементы, если список слишком мал
        }
        while (subList.Count > targetSize)
        {
            subList.RemoveAt(subList.Count - 1); // Удаляем элементы, если список слишком велик
        }
    }

    private void SaveGameResources(GameResources gameResources)
    {
        EditorUtility.SetDirty(gameResources);
        AssetDatabase.SaveAssets();
        Debug.Log("GameResources saved!");
    }
}
