using UnityEngine;
using UnityEngine.InputSystem;

public class Debugger : MonoBehaviour
{
    public GameObject enemyPrefab;

    public bool debugToolsOn;

    void Update()
    {
        
    }

    public void SpawnEnemy(InputAction.CallbackContext context)
    {
        if (context.ReadValueAsButton() && debugToolsOn)
        {
            Vector3 camMouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            camMouse.z = 0f;
            Instantiate(enemyPrefab, camMouse, Quaternion.identity);
        }
    }
}