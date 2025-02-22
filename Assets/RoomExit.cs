using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomExit : Interactable
{
    public override void Interact()
    {
        base.Interact();
        GameManager.instance.score += 300;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
