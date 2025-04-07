using UnityEngine;
using TMPro;
using System.Collections;

[CreateAssetMenu(fileName = "NewDialogue", menuName = "Dialogue/Dialogue Data")]
public class DialogueData : ScriptableObject
{
    [TextArea(3, 10)] public string[] dialogueLines;
}

public class DialogueSystem : MonoBehaviour
{
    public static DialogueSystem Instance;
    
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

    private void Awake()
    {
        Instance = this;
        
        _audioSource = GetComponent<AudioSource>();
        if (_audioSource == null)
        {
            _audioSource = gameObject.AddComponent<AudioSource>();
            _audioSource.playOnAwake = false;
        }
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

    private void DisplayNextLine()
    {
        if (_isTyping)
        {
            StopCoroutine(_typingCoroutine);
            _isTyping = false;
            dialogueText.text = _currentDialogue.dialogueLines[_currentLineIndex];
            return;
        }

        if (_currentLineIndex >= _currentDialogue.dialogueLines.Length)
        {
            EndDialogue();
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
        DisplayNextLine();
    }

    public void EndDialogue()
    {
        dialoguePanel.SetActive(false);
        
        if (_autoNextCoroutine != null)
            StopCoroutine(_autoNextCoroutine);
    }
}
