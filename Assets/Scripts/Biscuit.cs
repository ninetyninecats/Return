using UnityEngine;

public class Biscuit : MonoBehaviour
{
    [SerializeField] int biscuitNumber;
    void Start()
    {
        if (SaveFile.GetBiscuit(biscuitNumber)) Destroy(gameObject);
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        SaveFile.SetBiscuit(true, biscuitNumber);
        Destroy(gameObject);
    }
}
