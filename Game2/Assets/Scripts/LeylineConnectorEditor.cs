using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(LeylineConnectorBehavior))]
public class LeylineConnectorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        LeylineConnectorBehavior ll = (LeylineConnectorBehavior)target;
        if (GUILayout.Button("Init"))
        {
            ll.Init();
        }
    }
}
