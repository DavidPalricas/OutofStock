using UnityEngine;

public class TaskIcon : MonoBehaviour
{
    [SerializeField]
    private MeshRenderer showIcon;

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
           showIcon.enabled = false;
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            showIcon.enabled = true;
        }
    }
    private void Update()
    {
        transform.Rotate(0, 50 * Time.deltaTime, 0);
    }
}
