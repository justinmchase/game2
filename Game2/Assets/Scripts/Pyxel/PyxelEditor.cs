using System;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEditor.Experimental.AssetImporters;

// Example code:
// https://raw.githubusercontent.com/unity3d-jp/AlembicForUnity/master/AlembicImporter/Assets/UTJ/Alembic/Editor/Importer/AlembicImporterEditor.cs

namespace Game.Pyxel
{
    [CustomEditor(typeof(PyxelImporter))] // CanEditMultipleObjects]
    public class PyxelImporterEditor : ScriptedImporterEditor
    {
        public override void OnInspectorGUI()
        {
            var importer = serializedObject.targetObject as PyxelImporter;

            EditorGUILayout.LabelField("Sprites", EditorStyles.boldLabel);
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(
                  serializedObject.FindProperty("PixelsPerUnit"),
                  new GUIContent("Pixels Per Unit", "How many pixels per unit should be applied to generated Sprites.")
                );
                EditorGUILayout.PropertyField(
                  serializedObject.FindProperty("Pivot"),
                  new GUIContent("Pivot", "Where the pivot point will be located on generated Sprites.")
                );
                EditorGUILayout.PropertyField(
                  serializedObject.FindProperty("SpriteMaterial"),
                  new GUIContent("Sprite Material", "The Material to use for sprites.")
                );
                EditorGUILayout.PropertyField(
                    serializedObject.FindProperty("AnimatedFramesOnly"),
                    new GUIContent("Animated Frames Only", "Only export sprites for frames that exist in an animation")
                );
                EditorGUI.indentLevel--;
            }
            EditorGUILayout.Separator();

            EditorGUILayout.LabelField("Prefab", EditorStyles.boldLabel);
            {
                EditorGUI.indentLevel++;
                DisplayEnumProperty(serializedObject.FindProperty("BodyType"), Enum.GetNames(typeof(RigidbodyType2D)));
                EditorGUILayout.PropertyField(
                  serializedObject.FindProperty("ColliderOffset"),
                  new GUIContent("Collider Offset", "The offset of the Box Collider 2D.")
                );
                EditorGUILayout.PropertyField(
                  serializedObject.FindProperty("ColliderSize"),
                  new GUIContent("Collider Size", "The size of the Box Collider 2D.")
                );
                EditorGUILayout.PropertyField(
                  serializedObject.FindProperty("MapToAnimatorController"),
                  new GUIContent("Animator Controller", "The animator controller to clone and map this thing's animations to")
                );
                EditorGUI.indentLevel--;
            }
            EditorGUILayout.Separator();
            base.ApplyRevertGUI();
        }

        private static void DisplayEnumProperty(SerializedProperty prop, string[] displayNames, GUIContent guicontent = null)
        {
            if (guicontent == null)
                guicontent = new GUIContent(prop.displayName);

            var rect = EditorGUILayout.GetControlRect();
            EditorGUI.BeginProperty(rect, guicontent, prop);
            EditorGUI.showMixedValue = prop.hasMultipleDifferentValues;
            EditorGUI.BeginChangeCheck();

            GUIContent[] options = new GUIContent[displayNames.Length];
            for (int i = 0; i < options.Length; ++i)
                options[i] = new GUIContent(ObjectNames.NicifyVariableName(displayNames[i]), "");

            var normalsModeNew = EditorGUI.Popup(rect, guicontent, prop.intValue, options);
            if (EditorGUI.EndChangeCheck())
                prop.intValue = normalsModeNew;

            EditorGUI.showMixedValue = false;
            EditorGUI.EndProperty();
        }
    }
}
