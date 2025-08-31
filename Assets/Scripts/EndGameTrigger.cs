using UnityEngine;

public class EndGameTrigger : MonoBehaviour
{
    GameObject endScreen;
    void Start()
    {
        endScreen = GameObject.FindWithTag("Canvas").transform.GetChild(3).gameObject;
    }
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
