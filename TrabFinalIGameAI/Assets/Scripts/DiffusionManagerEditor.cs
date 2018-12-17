#if UNITY_EDITOR
using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(DiffusionManager))]
public class DiffusionManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        
        DiffusionManager myScript = (DiffusionManager)target;
        if(GUILayout.Button("Set Initial Matrix"))
        {
            myScript.SetMatrixPositions();
        }
    }
}
#endif