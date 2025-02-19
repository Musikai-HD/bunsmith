using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Debugger : MonoBehaviour
{
    public GameObject enemyPrefab, chestPrefab;

    public bool debugToolsOn;
    public CanvasGroup debugToolsMenu;

    void Update()
    {
        
    }


    //DEBUG MENU BUTTONS
    public void DebugRandomItem(ItemPool pool)
    {
        ItemWrapper item = ItemPicker.PickItem(pool);
        Debug.Log($"Got a {item.rarity.ToString()} item: {item.name} (Type: {item.GetType()})");
    }

    public void ResetGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void SpawnChestOnPlayer()
    {
        Instantiate(chestPrefab, GameManager.instance.pc.transform.position, Quaternion.identity);
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

    public void DebugMenu(InputAction.CallbackContext context)
    {
        if (context.ReadValueAsButton() && debugToolsOn)
        {
            debugToolsMenu.alpha = debugToolsMenu.alpha == 1f ? 0f : 1f;
            debugToolsMenu.blocksRaycasts = debugToolsMenu.alpha == 1f;
            debugToolsMenu.interactable = debugToolsMenu.alpha == 1f;
        }
    }

}