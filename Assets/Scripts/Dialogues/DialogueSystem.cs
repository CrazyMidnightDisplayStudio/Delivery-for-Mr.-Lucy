using UnityEngine;
using TMPro;
using System.Collections;
using MrLucy;

[CreateAssetMenu(fileName = "NewDialogue", menuName = "Dialogue/Dialogue Data")]
public class DialogueData : ScriptableObject
{
    [TextArea(3, 10)] public string[] dialogueLines;
}

public class DialogueSystem : Singleton<DialogueSystem>
{
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private float lineDelay = 1.5f;
    [SerializeField] private float typingSpeed = 0.05f;
    [SerializeField] private AudioClip typingSound;
    [SerializeField] private float typingSoundInterval = 0.1f;

    private DialogueData _currentDialogue;
    private int _currentLineIndex;
    private bool _isTyping;
    private Coroutine _typingCoroutine;
    private Coroutine _autoNextCoroutine;
    private AudioSource _audioSource;
    private float _lastTypingSoundTime;

    protected override void Awake()
    {
        base.Awake();
        _audioSource = GetComponent<AudioSource>();
        _audioSource.playOnAwake = false;
    }

    private void Start()
    {
        dialoguePanel.SetActive(false);
    }

    public void StartDialogue(DialogueData dialogue)
    {
        _currentDialogue = dialogue;
        _currentLineIndex = 0;
        dialoguePanel.SetActive(true);
        DisplayNextLine();
    }

    public void StartDialogueJump()
    {
        string text = "Jump!";
        PrintSingleMessage(text);
    }

    public void StartDialogueFind()
    {
        string text = "The button fell off, I need to find it!";
        PrintSingleMessage(text);
    }

    public void PrintSingleMessage(string message)
    {
        // Сброс текущего состояния
        if (_typingCoroutine != null)
            StopCoroutine(_typingCoroutine);

        if (_autoNextCoroutine != null)
            StopCoroutine(_autoNextCoroutine);

        _isTyping = false;
        _currentDialogue = null; // Без ScriptableObject

        // Показываем панель
        dialoguePanel.SetActive(true);

        // Печатаем текст
        _typingCoroutine = StartCoroutine(TypeText(message));
    }

    private void DisplayNextLine()
    {
        if (_currentDialogue == null || _currentDialogue.dialogueLines == null)
        {
            Debug.LogWarning("No active dialogue to display.");
            EndDialogue();
            return;
        }

        if (_currentLineIndex >= _currentDialogue.dialogueLines.Length)
        {
            EndDialogue();
            return;
        }

        if (_isTyping)
        {
            StopCoroutine(_typingCoroutine);
            _isTyping = false;
            dialogueText.text = _currentDialogue.dialogueLines[_currentLineIndex];
            return;
        }

        _typingCoroutine = StartCoroutine(TypeText(_currentDialogue.dialogueLines[_currentLineIndex]));
        _currentLineIndex++;
    }

    private IEnumerator TypeText(string text)
    {
        _isTyping = true;
        dialogueText.text = "";

        for (int i = 0; i < text.Length; i++)
        {
            dialogueText.text += text[i];

            if (typingSound != null && Time.time - _lastTypingSoundTime >= typingSoundInterval)
            {
                _audioSource.PlayOneShot(typingSound);
                _lastTypingSoundTime = Time.time;
            }

            yield return new WaitForSeconds(typingSpeed);
        }

        _isTyping = false;

        if (_autoNextCoroutine != null)
            StopCoroutine(_autoNextCoroutine);

        _autoNextCoroutine = StartCoroutine(AutoNextLine());
    }

    private IEnumerator AutoNextLine()
    {
        yield return new WaitForSeconds(lineDelay);

        if (_currentDialogue == null)
        {
            EndDialogue();
            yield break;
        }

        DisplayNextLine();
    }

    public void EndDialogue()
    {
        dialoguePanel.SetActive(false);

        if (_autoNextCoroutine != null)
            StopCoroutine(_autoNextCoroutine);
    }
}