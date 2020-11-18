using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    #region Singleton
    static DialogueManager instance = null;
    public static DialogueManager Instance => instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    #endregion Singleton

    #region Variables
    public Text nameText = null;                // 이름 텍스트
    public Text dialogueText = null;            // 대화 텍스트

    public Animator animator = null;            // 애니메이터

    // 문장을 저장할 Queue
    Queue<string> sentences = new Queue<string>();

    // 대화 시작시 호출될 이벤트
    public event System.Action OnStartDialogue = null;
    // 대화 종료시 호출될 이벤트
    public event System.Action OnEndDialogue = null;
    #endregion Variables

    #region Main Methods
    /// <summary>
    /// 대화를 시작하는 함수
    /// </summary>
    /// <param name="dialogue">다이얼로그</param>
    public void StartDialogue(Dialogue dialogue)
    {
        // 시작시 실행할 이벤트 함수 호출
        OnStartDialogue?.Invoke();

        // UI 오픈 애니메이션 재생
        animator?.SetBool("IsOpen", true);

        nameText.text = dialogue.name;
        // Queue 초기화
        sentences.Clear();

        // 문장들을 Queue에 적재
        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        // 다음 문장 출력
        DisplayNextSentence();
    }

    /// <summary>
    /// 다음 문장을 출력하는 함수
    /// </summary>
    public void DisplayNextSentence()
    {
        // 문장이 더이상 없다면 대화 종료
        if(sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        // 문장 출력
        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    /// <summary>
    /// 문자 단위로 문장을 출력하는 코루틴 함수
    /// </summary>
    /// <param name="sentence">문장</param>
    /// <returns></returns>
    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = string.Empty;

        // 애니메이션 완료시까지 딜레이
        yield return new WaitForSeconds(0.25f);

        // 프레임 단위로 문장을 문자단위로 출력
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return null;
        }
    }

    /// <summary>
    /// 대화를 종료하는 함수
    /// </summary>
    void EndDialogue()
    {
        // UI 클로즈 애니메이션 재생
        animator?.SetBool("IsOpen", false);

        // 종료 시 수행할 이벤트 함수 호출
        OnEndDialogue?.Invoke();
    }
    #endregion Main Methods
}
