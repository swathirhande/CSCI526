using UnityEngine;
using UnityEngine.UI;

public class PrefabToImage : MonoBehaviour
{
    public GameObject prefab; // ????Prefab
    public Image targetImage; // ????Image??

    void Start()
    {
        // ?Prefab???Sprite??
        Sprite sprite = prefab.GetComponentInChildren<SpriteRenderer>().sprite;

        // ?Sprite???Image??
        targetImage.sprite = sprite;
    }
}
