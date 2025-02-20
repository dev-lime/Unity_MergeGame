using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

[CustomEditor(typeof(GameResources))]
public class GameResourcesEditor : Editor
{
    private int subListSize = 4; // ������ ��������� ������� (�� ��������� 4)
    private Dictionary<int, bool> foldoutStates = new Dictionary<int, bool>(); // ��������� ������������ ��� ������� ������

    public override void OnInspectorGUI()
    {
        // �������� ������ �� ������ GameResources
        GameResources gameResources = (GameResources)target;

        // ���������� subListSize �� GameResources
        gameResources.subListSize = EditorGUILayout.IntField("Sub List Size", gameResources.subListSize);
        gameResources.subListSize = Mathf.Max(1, gameResources.subListSize); // ������� 1

        // ���������� ������ ��������� ������
        for (int i = 0; i < gameResources.items.Count; i++)
        {
            // ������������� ��������� ������������ ��� �������� ������
            if (!foldoutStates.ContainsKey(i))
                foldoutStates[i] = false; // �� ��������� ������ ������

            // ���������� ��������� ������ � ������� ������������
            foldoutStates[i] = EditorGUILayout.Foldout(foldoutStates[i], $"Item List {i}", true);

            // ���� ������ ��������, ���������� ��� ��������
            if (foldoutStates[i])
            {
                EditorGUI.indentLevel++; // ����������� ������ ��� ��������� ���������

                // ��������, ��� ������ ���������� ������ ������������� subListSize
                ResizeSubList(gameResources.items[i], gameResources.subListSize);

                // ���������� ���� ��� ������� �������� ���������� ������
                for (int j = 0; j < gameResources.items[i].Count; j++)
                {
                    gameResources.items[i][j] = (Sprite)EditorGUILayout.ObjectField(
                        $"Element {j}",
                        gameResources.items[i][j],
                        typeof(Sprite),
                        false
                    );
                }

                // ������ ��� �������� �������� ������
                if (GUILayout.Button("Remove This List"))
                {
                    gameResources.items.RemoveAt(i);
                    foldoutStates.Remove(i); // ������� ��������� ������������ ��� ��������� ������
                    i--; // ������������ ������ ����� ��������
                    SaveGameResources(gameResources); // ��������� ���������
                    break; // ������� �� �����, ����� �������� ������
                }

                EditorGUI.indentLevel--; // ��������� ������
            }

            EditorGUILayout.Space(); // ��������� ������ ����� ��������
        }

        EditorGUILayout.Separator();
        // ������ ��� ���������� ������ ������
        if (GUILayout.Button(new GUIContent(" Add New List", EditorGUIUtility.IconContent("d_Toolbar Plus").image)))
        {
            AddNewList(gameResources);
        }

        // ��������� ���������, ���� ��� ����
        if (GUI.changed)
        {
            SaveGameResources(gameResources);
        }
    }

    // ����� ��� ���������� ������ ������
    private void AddNewList(GameResources gameResources)
    {
        List<Sprite> newList = new List<Sprite>();
        for (int i = 0; i < subListSize; i++)
        {
            newList.Add(null); // �������������� ������ ������� ��������� null
        }
        gameResources.items.Add(newList);
        SaveGameResources(gameResources); // ��������� ���������
    }

    // ����� ��� ��������� ������� ���������� ������
    private void ResizeSubList(List<Sprite> subList, int targetSize)
    {
        while (subList.Count < targetSize)
        {
            subList.Add(null); // ��������� ��������, ���� ������ ������� ���
        }
        while (subList.Count > targetSize)
        {
            subList.RemoveAt(subList.Count - 1); // ������� ��������, ���� ������ ������� �����
        }
    }

    private void SaveGameResources(GameResources gameResources)
    {
        EditorUtility.SetDirty(gameResources);
        AssetDatabase.SaveAssets();
        Debug.Log("GameResources saved!");
    }
}
