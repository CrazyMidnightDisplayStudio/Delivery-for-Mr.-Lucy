using UnityEngine;

namespace MrLucy
{
public class Code312 : MonoBehaviour
{
    [SerializeField] private string correctCode = "312";
    private string currentInput = "";
    
    public bool active = false;

    public void AddDigit(int digit)
    {
        if (!active) return;
        
        currentInput += digit.ToString();
        Debug.Log("Код сейчас: " + currentInput);

        if (!correctCode.StartsWith(currentInput))
        {
            Debug.Log("Ошибка во вводе. Сброс.");
            currentInput = "";
            return;
        }

        if (currentInput == correctCode)
        {
            Debug.Log("Код правильный!");
            OnCorrectCodeEntered();
            currentInput = "";
        }
    }

    private void OnCorrectCodeEntered()
    {
        GameManager.Instance.SetState(GameState.FinalScene);
    }
}
}