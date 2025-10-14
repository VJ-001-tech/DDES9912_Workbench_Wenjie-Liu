using TMPro;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public class SetZi : MonoBehaviour
{
    //public Transform[] allTfs;

    [MenuItem("Tools/SetZi")]
    public static void SetNames()
    {
        TextMeshPro[] tmps = Selection.activeTransform.GetComponentsInChildren<TextMeshPro>();
        string info = "QWERTYUIOPASDFGHJKLZXCVBNM,.";
        for (int i = 0; i < tmps.Length; i++)
        {
            if (i < info.Length)
            {
                tmps[i].text = info[i].ToString();
                EditorUtility.SetDirty(tmps[i]);
            }
        }
    }
}
