using log4net.Util;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(FieldOfView))]
public class FieldOfViewEditor : Editor
{
    private void OnSceneGUI()
    {
        FieldOfView fov = (FieldOfView)target;
        var viewingPosition = new Vector3(fov.transform.position.x, fov.transform.position.y + 1.3f, fov.transform.position.z);
        Handles.color = Color.white;
        Handles.DrawWireArc(viewingPosition, Vector3.up, Vector3.forward, 360, fov.radius);

        Vector3 viewAngle01 = DirectionFromAngle(fov.transform.eulerAngles.y, -fov.detectionAngle / 2);
        Vector3 viewAngle02 = DirectionFromAngle(fov.transform.eulerAngles.y, fov.detectionAngle / 2);

        Handles.color = Color.yellow;
        Handles.DrawLine(viewingPosition, viewingPosition + viewAngle01 * fov.radius);
        Handles.DrawLine(viewingPosition, viewingPosition + viewAngle02 * fov.radius);

        if (fov.canSeePlayer)
        {
            Handles.color = Color.green;

            // adjust angle to 1.3f (around middle of body of target)
            var targetViewingPosition = new Vector3(fov.playerRef.transform.position.x, fov.playerRef.transform.position.y + 1.3f, fov.playerRef.transform.position.z);
            Handles.DrawLine(viewingPosition, targetViewingPosition);
        }
    }

    private Vector3 DirectionFromAngle(float eulerY, float angleInDegrees)
    {
        angleInDegrees += eulerY;

        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}