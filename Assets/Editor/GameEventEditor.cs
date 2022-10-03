using log4net.Config;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro.EditorUtilities;
using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

[CustomEditor(typeof(GameEvent))]

public class GameEventEditor : Editor
{
    List<bool> _feedbacksFoldout;
    SerializedProperty _feedback;

    string[] _typesStr;
    System.Type[] _types;

    int _dragStartID;
    int _dragEndID;

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        //Futur code
        int controlID = GUIUtility.GetControlID(FocusType.Passive);

        GameEvent gameEvent = target as GameEvent;

        string[] types = new string[] { "add new feedback", "Instantiate", "Wait" };

        int newItem = EditorGUILayout.Popup(0, _typesStr) - 1;
        if (newItem >= 0)
        {
            GameFeedback newFeedback = Activator.CreateInstance(_types[newItem]) as GameFeedback;
           
            Undo.RecordObject(gameEvent, "Add feedback");
            gameEvent.gameFeedbacks.Add(newFeedback);
            _feedbacksFoldout.Add(false);
        }

        for (int i = 0; i < _feedback.arraySize; i++)
        {
            SerializedProperty property = _feedback.GetArrayElementAtIndex(i);

            Rect horizontal = EditorGUILayout.BeginHorizontal();

            var backgroundRect = GUILayoutUtility.GetRect(5f, 17f);
            var offset = 4f;
            backgroundRect.xMax = 5;
            backgroundRect.xMin = 0;
            var foldoutRect = backgroundRect;
            foldoutRect.xMin += offset;
            foldoutRect.width = 20;
            foldoutRect.height = 17;
            var line = GUILayoutUtility.GetRect(1f, 1f);
            EditorGUI.DrawRect(line, Color.black);

            EditorGUI.DrawRect(backgroundRect, Color.white);

            _feedbacksFoldout[i] = GUI.Toggle(foldoutRect, _feedbacksFoldout[i], gameEvent.gameFeedbacks[i].ToString(), EditorStyles.foldout);

            int indexRemove = -1;

            if(GUILayout.Button("X",EditorStyles.miniButton, GUILayout.Width(EditorStyles.miniButton.CalcSize(new GUIContent("X")).x)))
            {
                indexRemove = i;

            }

            EditorGUILayout.EndHorizontal();

            if (_feedbacksFoldout[i])
            {
                foreach (var item in GetChildren(property))
                {
                    EditorGUILayout.PropertyField(item);
                }
            }

            if (indexRemove != -1)
            {
                _feedback.DeleteArrayElementAtIndex(indexRemove);
                _feedbacksFoldout.RemoveAt(indexRemove);
            }

            var eventCurrent = Event.current;
            if(eventCurrent.type == EventType.MouseDown)
            {
                GUIUtility.hotControl = controlID;
                if (horizontal.Contains(eventCurrent.mousePosition))
                {
                    _dragStartID = i;
                    eventCurrent.Use(); 
                }
            }

            if(_dragStartID == i)
            {
                Color color = new Color(0, 1, 0, 0.3f);
                EditorGUI.DrawRect(horizontal, color);
            }

            if (horizontal.Contains(eventCurrent.mousePosition))
            {
                if(_dragStartID >= 0)
                {
                    _dragEndID = i;

                    Rect headerSplit = horizontal;
                    headerSplit.height *= 0.5f;

                    headerSplit.y += headerSplit.height;
                    if (headerSplit.Contains(eventCurrent.mousePosition))
                    {
                        _dragEndID = i + 1;
                    }
                }
            }

            if(_dragStartID >=0 && _dragEndID >= 0)
            {
                if (_dragStartID != _dragEndID)
                {
                    if (_dragEndID > _dragStartID)
                        _dragEndID--;
                    _feedback.MoveArrayElement(_dragStartID, _dragEndID);
                    _dragStartID = _dragEndID;
                }
            }

            if(_dragStartID >= 0 || _dragEndID >= 0)
            {
                if(eventCurrent.type == EventType.MouseUp)
                {
                    _dragStartID = -1;
                    _dragEndID = -1;
                    eventCurrent.Use();
                }
            }
        }

        serializedObject.ApplyModifiedProperties();
    }

    private void OnEnable()
    {

        List<string> types = new List<string>();
        types.Add("Add new feedback");

        List<System.Type> gameEventTypes = (from domainAssembly in System.AppDomain.CurrentDomain.GetAssemblies()
                                            from assemblyType in domainAssembly.GetTypes()
                                            where assemblyType.IsSubclassOf(typeof(GameFeedback))
                                            select assemblyType).ToList();

        _types = gameEventTypes.ToArray();

        foreach(var item in gameEventTypes)
        {
            types.Add(item.ToString());
        }

        _typesStr = types.ToArray();

        _feedbacksFoldout = new List<bool>();
        _feedback = serializedObject.FindProperty("gameFeedbacks");

        for(int i = 0 ; i < _feedback.arraySize; i++)
        {
            _feedbacksFoldout.Add(false);
        }
    }

    public IEnumerable<SerializedProperty> GetChildren(SerializedProperty serializedProperty)
    {
        SerializedProperty currentProperty = serializedProperty.Copy();
        SerializedProperty nextSiblingProperty = serializedProperty.Copy();
        {
            nextSiblingProperty.Next(false);
        }
        if (currentProperty.Next(true))
        {
            do
            {
                if (SerializedProperty.EqualContents(currentProperty, nextSiblingProperty))
                    break;

                yield return currentProperty; 
            }
            while(currentProperty.Next(false));
        }
    }
}
