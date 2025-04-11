using UnityEngine;
using UnityEngine.InputSystem;

public class InteractSubTask : SubTask
{
    [SerializeField]
    private InputActionReference subTaskInput;

    [SerializeField]
    private RectTransform crosshair;

    private float subTaskTime;

    private float currentTaskTime;

    public bool CanBeVanish { get; set; } = false;

    public bool CanBeReseted { get; set; } = false;


    private void OnEnable()
    {
        subTaskTime = Utils.RandomFloat(1f, 5f);
        currentTaskTime = 0f;
        type = SubTaskType.Interact;
    }

    private void Update()
    {
        if (IsPlayerDoingSubTask())
        {
            UpdateSubTaskProgress();
        }
        else
        {
            if (CanBeReseted)
            {
                currentTaskTime = 0f;
            }
        }
    }

    private bool IsPlayerDoingSubTask()
    {
        if (!subTaskInput.action.IsPressed())
        {
            return false;
        }

        if (subTaskGameObject == null)
        {
            subTaskGameObject = gameObject;
        }

        Ray ray = Utils.CastRayFromUI(crosshair);

        const float RAYCASTDISTANCE = 3f;

        return Physics.Raycast(ray, out RaycastHit hit, RAYCASTDISTANCE, LayerMask.GetMask("Default")) && hit.collider.gameObject == subTaskGameObject;
    }

    private void UpdateSubTaskProgress()
    {
        currentTaskTime += Time.deltaTime;

        Debug.Log("SubTask Progress: " + (currentTaskTime / subTaskTime) * 100 + "%");

        if (currentTaskTime >= subTaskTime)
        {
            SubTaskCompleted();

            if (CanBeVanish)
            {
                gameObject.SetActive(false);
            }

            enabled = false;
        }
    }
}
