using UnityEngine;

public class FinalText : MonoBehaviour
{
    [SerializeField] private GameObject textObject;

    public void TextApear()
    {
        textObject.SetActive(true);
    }
}
