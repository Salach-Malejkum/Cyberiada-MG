using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public class AltDuplicateHandle
{
    private static GameObject duplicatedObject = null;
    private static bool isDragging = false;
    private static Vector3 dragStartPosition;
    private static Vector3 dragAxis = Vector3.zero;
    private static Plane dragPlane;
    
    static AltDuplicateHandle()
    {
        SceneView.duringSceneGui += OnSceneGUI;
    }

    private static void OnSceneGUI(SceneView sceneView)
    {
        Event e = Event.current;
        if (Selection.activeGameObject == null) return;

        if (e.alt && e.type == EventType.MouseDrag && e.button == 0)
        {
            if (duplicatedObject == null)
            {
                duplicatedObject = Object.Instantiate(Selection.activeGameObject);
                duplicatedObject.name = Selection.activeGameObject.name + " (Copy)";
                Undo.RegisterCreatedObjectUndo(duplicatedObject, "Duplicate Object");
                Selection.activeGameObject = duplicatedObject;
            }

            dragStartPosition = duplicatedObject.transform.position;
            isDragging = true;

            float handleSize = HandleUtility.GetHandleSize(dragStartPosition);
            float pickDist = handleSize * 0.1f;

            if (HandleUtility.DistanceToLine(dragStartPosition, dragStartPosition + Vector3.right * handleSize) < pickDist)
                dragAxis = Vector3.right;
            else if (HandleUtility.DistanceToLine(dragStartPosition, dragStartPosition + Vector3.up * handleSize) < pickDist)
                dragAxis = Vector3.up;
            else if (HandleUtility.DistanceToLine(dragStartPosition, dragStartPosition + Vector3.forward * handleSize) < pickDist)
                dragAxis = Vector3.forward;
            else
            {
                dragPlane = new Plane(Vector3.forward, dragStartPosition);
                dragAxis = Vector3.zero;
            }

            e.Use();
        }

        if (duplicatedObject != null)
        {
            EditorGUI.BeginChangeCheck();
            Vector3 newPosition = duplicatedObject.transform.position;

            if (dragAxis != Vector3.zero)
            {
                newPosition = Handles.Slider(duplicatedObject.transform.position, dragAxis);
            }
            else
            {
                Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
                if (dragPlane.Raycast(ray, out float enter))
                {
                    newPosition = ray.GetPoint(enter);
                }
            }

            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(duplicatedObject.transform, "Move Duplicated Object");
                duplicatedObject.transform.position = newPosition;
            }

            sceneView.Repaint();
        }

        if (e.type == EventType.MouseUp)
        {
            duplicatedObject = null;
            isDragging = false;
            dragAxis = Vector3.zero;
        }

        if (isDragging && e.isMouse)
        {
            e.Use();
        }
    }
}
