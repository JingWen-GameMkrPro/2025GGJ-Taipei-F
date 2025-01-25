using UnityEngine;
using UnityEditor;

public class SimplePrefabSorter : EditorWindow
{
	public Transform targetParent;  // 指定的父物件

	[MenuItem("Tools/Simple Prefab Sorter")]
	public static void ShowWindow()
	{
		EditorWindow.GetWindow(typeof(SimplePrefabSorter), false, "Simple Prefab Sorter");
	}

	private void OnGUI()
	{
		GUILayout.Label("Prefab Sorter", EditorStyles.boldLabel);

		// 欄位讓使用者選擇目標Transform
		targetParent = (Transform)EditorGUILayout.ObjectField("Target Parent", targetParent, typeof(Transform), true);

		if (GUILayout.Button("Sort Children"))
		{
			SortChildren();
		}
	}

	private void SortChildren()
	{
		if (targetParent == null)
		{
			Debug.LogError("Target Parent is not assigned!");
			return;
		}

		// 取得所有直接的子物件
		Transform[] children = new Transform[targetParent.childCount];
		for (int i = 0; i < targetParent.childCount; i++)
		{
			children[i] = targetParent.GetChild(i);
		}

		// 根據位置 (X 和 Y) 進行排序
		System.Array.Sort(children, (child1, child2) =>
		{
			Vector3 pos1 = child1.position;
			Vector3 pos2 = child2.position;

			// 使用 X 和 Y 位置進行排序，從左至右、從上至下
			if (Mathf.Abs(pos1.x - pos2.x) > 0.1)
				return pos1.x.CompareTo(pos2.x);
			else
				return pos1.y.CompareTo(pos2.y);
		});

		// 根據位置 (X 和 Y) 進行排序
		System.Array.Sort(children, (child1, child2) =>
		{
			Vector3 pos1 = child1.position;
			Vector3 pos2 = child2.position;

			// 使用 X 和 Y 位置進行排序，從左至右、從上至下
			if (Mathf.Abs(pos1.y - pos2.y) > 0.1)
				return -pos1.y.CompareTo(pos2.y);
			else
				return pos1.x.CompareTo(pos2.x);
		});

		// 重新設定每個子物件的層級順序
		for (int i = 0; i < children.Length; i++)
		{
			children[i].SetSiblingIndex(i);
		}

		Debug.Log("Children sorted!");
	}
}