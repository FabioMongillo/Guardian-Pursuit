using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Guardian2))]

public class FovEditor : Editor
{
    private void OnSceneGUI()
    {
        //Drawing to see player
        Guardian2 fov = (Guardian2)target;
        Handles.color = Color.blue;
        Handles.DrawWireArc(fov.transform.position, Vector3.up, Vector3.forward, 360, fov.radius);

        Vector3 viewAngle1 = directionFromAngle(fov.transform.eulerAngles.y, -fov.fovAngle / 2);
        Vector3 viewAngle2 = directionFromAngle(fov.transform.eulerAngles.y, fov.fovAngle / 2);

        Handles.color = Color.yellow;
        Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngle1 * fov.radius);
        Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngle2 * fov.radius);

        if (fov.canSeePlayer)
        {
            Handles.color = Color.green;
            Handles.DrawLine(fov.transform.position, fov.playerRef.transform.position);
        }
        

    }

    private Vector3 directionFromAngle(float eulerY, float angleInDegrees)
    {
        angleInDegrees += eulerY;

        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }

}
