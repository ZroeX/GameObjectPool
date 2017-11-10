using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ONIK.ObjectPool.Editor {
    [CustomEditor( typeof( EffectPool ) )]
    public class EffectPoolInspector : UnityEditor.Editor {
        private SerializedProperty poolPrefabListProperty;
        private List<bool> showItemSlots = new List<bool>();

        private static readonly float BUTTON_WIDTH = 125f;

        private void OnEnable() {
            poolPrefabListProperty = serializedObject.FindProperty( "poolPrefabs" );
            for ( int i = 0; i < poolPrefabListProperty.arraySize; ++i ) {
                showItemSlots.Add( false );
            }
        }

        public sealed override void OnInspectorGUI() {
            serializedObject.Update();

            if ( Application.isPlaying ) {
                GUI.enabled = false;
            }

            for ( int i = 0; i < poolPrefabListProperty.arraySize; ++i ) {
                ItemSlotGUI( i );
            }

            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if ( GUILayout.Button( "Add Effect", GUILayout.Width( BUTTON_WIDTH ) ) ) {
                ++poolPrefabListProperty.arraySize;
                showItemSlots.Add( false );
            }
            EditorGUILayout.EndHorizontal();

            GUI.enabled = true;

            serializedObject.ApplyModifiedProperties();
        }

        private void ItemSlotGUI( int index ) {
            EditorGUILayout.BeginVertical( GUI.skin.box );
            ++EditorGUI.indentLevel;

            // Display a foldout to determine whether the GUI should be shown or not.
            SerializedProperty poolablePrefabPoolData = poolPrefabListProperty.GetArrayElementAtIndex( index );
            showItemSlots[ index ] = EditorGUILayout.Foldout( showItemSlots[ index ],
                ( index + 1 ).ToString() + ". " + poolablePrefabPoolData.FindPropertyRelative( "name" ).stringValue );

            // If the foldout is open then display default GUI for the specific elements in each array.
            if ( showItemSlots[ index ] ) {
                EditorGUILayout.PropertyField( poolablePrefabPoolData.FindPropertyRelative( "name" ) );
                EditorGUILayout.PropertyField( poolablePrefabPoolData.FindPropertyRelative( "defaultNumber" ) );
                EditorGUILayout.PropertyField( poolablePrefabPoolData.FindPropertyRelative( "prefabs" ), true );

                EditorGUILayout.Space();
                if ( GUILayout.Button( "Remove Effect", GUILayout.Width( BUTTON_WIDTH ) ) ) {
                    poolPrefabListProperty.DeleteArrayElementAtIndex( index );
                    showItemSlots.RemoveAt( index );
                }
            }

            --EditorGUI.indentLevel;
            EditorGUILayout.EndVertical();
        }
    }
}