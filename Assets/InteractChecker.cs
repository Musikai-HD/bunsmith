using System.Collections.Generic;
using UnityEngine;

public class InteractChecker : MonoBehaviour
{
    public Interactable focusedInteractable;
    public List<Interactable> interactables;

    void Awake()
    {
        interactables = new List<Interactable>();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        Interactable interactable = col.gameObject.GetComponent<Interactable>();
        interactables.Add(interactable);
        if (interactables.Count == 1) focusedInteractable = interactable;
    }

    void OnTriggerExit2D(Collider2D col)
    {
        Interactable interactable = col.gameObject.GetComponent<Interactable>();
        interactables.Remove(interactable);
        focusedInteractable = interactables.Count > 0 ? GetClosestInteractable() : null;
    } 

    void Update()
    {
        if (interactables.Count > 1)
        {
            focusedInteractable = GetClosestInteractable();
        }
    }

    public void Interact()
    {
        focusedInteractable?.Interact();
    }

    Interactable GetClosestInteractable()
    {
        Transform tMin = null;
        float minDist = Mathf.Infinity;
        Vector3 currentPos = transform.position;
        foreach (Interactable t in interactables)
        {
            float dist = Vector3.Distance(t.transform.position, currentPos);
            if (dist < minDist)
            {
                tMin = t.transform;
                minDist = dist;
            }
        }
        return tMin.GetComponent<Interactable>();
    }
}
