using UnityEditor; 
using UnityEngine;

[CustomEditor(typeof(MangerFov))] 
public class FOVEditor : Editor
{
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

            if (fov.CustomersHitted.Count > 0)
            {
                foreach (GameObject customerHitted in fov.CustomersHitted)
                {
                    Handles.color = Color.red;
                    Handles.DrawLine(fov.transform.position, customerHitted.transform.position);
                }       
            }
        }
    }


    private Vector3 DirectionFromAngle(float eulerY, float angleDegrees)
    {
        angleDegrees += eulerY;

        return new Vector3(Mathf.Sin(angleDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleDegrees * Mathf.Deg2Rad));
    }
}
