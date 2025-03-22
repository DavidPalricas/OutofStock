using UnityEditor; 
using UnityEngine;

[CustomEditor(typeof(MangerFov))]

/// <summary>
/// The FOVEditor class is used to represent in the unity editor the field of view of the manager by handling if detects a player and one more customer on his field of view.
/// </summary>
public class FOVEditor : Editor
{
    /// <summary>
    /// The OnSceneGUI method is called by the editor when the scene is being drawn (Unity Callback).
    /// </summary>
    /// <remarks>
    /// In this method, the manager radius and his field of view for each eye side (DirectionFromAngleMethod) of the manager is drawn in the scene view.
    /// And if the manager sees the targets, is drawn a green line towards the player and red lines towards the customers.
    /// </remarks>
    private void OnSceneGUI()
    {
        MangerFov fov = (MangerFov)target;
        Handles.color = Color.white;
        Handles.DrawWireArc(fov.transform.position, Vector3.up, Vector3.forward, 360, fov.radius);

        Vector3 leftViewAngle = DirectionFromAngle(fov.transform.eulerAngles.y, -fov.angle / 2);

        Vector3 rightViewAngle = DirectionFromAngle(fov.transform.eulerAngles.y, fov.angle / 2);

        Handles.color = Color.yellow;
        Handles.DrawLine(fov.transform.position, fov.transform.position + leftViewAngle * fov.radius);
        Handles.DrawLine(fov.transform.position, fov.transform.position + rightViewAngle * fov.radius);

        if (fov.TargetsSeen)
        {
            Handles.color = Color.green;
            Handles.DrawLine(fov.transform.position, fov.Player.transform.position);
  
           foreach (GameObject customerHitted in fov.CustomersHitted)
           {
                Handles.color = Color.red;
                Handles.DrawLine(fov.transform.position, customerHitted.transform.position);
           }       
            
        }
    }

    /// <summary>
    /// The DirectionFromAngle method is used to get the direction from an angle.
    /// </summary>
    /// <param name="eulerY">The euler y.</param>
    /// <param name="angleDegrees">The angle degrees.</param>
    /// <returns>A Vector3 representing the direction from the given angle.</returns>
    private Vector3 DirectionFromAngle(float eulerY, float angleDegrees)
    {
        angleDegrees += eulerY;

        return new Vector3(Mathf.Sin(angleDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleDegrees * Mathf.Deg2Rad));
    }
}
