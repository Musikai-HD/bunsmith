using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomExit : Interactable
{
    public override void Interact()
    {
        base.Interact();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
