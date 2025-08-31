using Unity.VisualScripting;
using UnityEngine;

public class EndGameTrigger : MonoBehaviour
{
    [SerializeField] GameObject endScreen;
    void OnTriggerEnter2D(Collider2D collision)
    {
        endScreen.SetActive(true);
        for (int ii = 0; ii < endScreen.transform.childCount; ii += 1)
        {
            GameObject biscuit = endScreen.transform.GetChild(ii).gameObject;
            biscuit.SetActive(SaveFile.GetBiscuit(ii));
        }
    }
}
