using UnityEngine;
using UnityEngine.InputSystem;

public class AimingAction : MonoBehaviour
{

    protected private const float RAYCASTDISTANCE = 2f;

    /// <summary>
    /// The crosshair attribute is the crosshair RectTransform.
    /// </summary>
    public RectTransform crosshair;

    [SerializeField]
    protected InputActionReference interactAction;
}
