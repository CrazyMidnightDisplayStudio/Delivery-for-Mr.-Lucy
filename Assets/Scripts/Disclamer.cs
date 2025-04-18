using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TerminalTextLoader : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI terminalText;
    [SerializeField] float typingSpeed = 0.05f;

    [SerializeField] private string[] lines;

    private bool isCutsceneEnd = false;

    void Start()
    {
        terminalText.text = "";

        StartCoroutine(TypeText("", false));
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space) || isCutsceneEnd)
            {
                LoadNextScene();
            }
    }

    IEnumerator TypeText(string line, bool clear)
    {
        if (clear)
            terminalText.text += "\n";

        foreach (char letter in line)
        {
            terminalText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
        yield return new WaitForSeconds(1f);
        StartCoroutine(TypeAllStrings());
    }

        IEnumerator TypeAllStrings()
    {
        foreach (string line in lines)
        {
            terminalText.text += "\n";
            foreach (char letter in line)
            {
                terminalText.text += letter;
                yield return new WaitForSeconds(typingSpeed);
            }
            yield return new WaitForSeconds(1f);
        }
        isCutsceneEnd = true;
    }

    private void LoadNextScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        if (nextSceneIndex >= SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0;
        }

        SceneManager.LoadScene(nextSceneIndex);
    }
}