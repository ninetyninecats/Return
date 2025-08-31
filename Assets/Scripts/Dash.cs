using UnityEngine;

public class Dash : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (SaveFile.GetDash()) Destroy(gameObject);
   
    }

    // Update is called once per frame
    void OnTriggerEnter2D(Collider2D collision)
    {
        SaveFile.SetDash(true);
        Destroy(gameObject);
    }
}
