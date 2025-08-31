using UnityEngine;

public class DoubleJump : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (SaveFile.GetDoubleJump()) Destroy(gameObject);
    }

    // Update is called once per frame
    void OnTriggerEnter2D(Collider2D collision)
    {
        SaveFile.SetDoubleJump(true);
        Destroy(gameObject);
    }
}
