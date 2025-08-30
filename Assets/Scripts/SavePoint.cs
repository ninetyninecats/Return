using UnityEngine;

public class SavePoint : MonoBehaviour
{
    [SerializeField] int savePoint;

    void OnTriggerEnter2D(Collider2D collision)
    {
        SaveFile.SetSavePoint(savePoint);
        SaveFile.SaveToFile();
    }
}
