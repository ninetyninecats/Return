using UnityEngine;

public class Biscuit : MonoBehaviour
{
    [SerializeField] int biscuitNumber;
    [SerializeField] AudioClip sound;
    void Start()
    {
        if (SaveFile.GetBiscuit(biscuitNumber)) gameObject.SetActive(false);
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        SaveFile.SetBiscuit(true, biscuitNumber);
        SoundManager.instance.PlaySound(sound);
        Destroy(gameObject);
    }
}
