using System.Collections;
using UnityEngine;

public class PhaseManager : MonoBehaviour
{
    enum Phase
    {
        gallery,
        aquarium,
        ad,
        feeding
    }

    Phase currentPhase;

    private void Update()
    {
        if (InputManager.selected != null)
            Debug.Log(InputManager.selected.name);
    }

    public void StartGallery(string player)
    {
        currentPhase = Phase.gallery;
        Debug.Log($"ギャラリー（{player}のターン）");
    }
}
