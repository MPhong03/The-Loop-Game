using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StoryLineController : MonoBehaviour
{
    public TextMeshProUGUI textMeshPro;
    public float delayBetweenCharacters = 0.05f;
    public float delayBetweenSentences = 3f;
    private float delayScenn = 2f;
    public string[] conversations;

    private void Start()
    {
        StartCoroutine(ShowConversations());
    }

    IEnumerator ShowConversations()
    {
        yield return new WaitForSeconds(delayScenn);

        foreach (string conversation in conversations)
        {
            yield return StartCoroutine(TypeSentence(conversation));
            yield return new WaitForSeconds(delayBetweenSentences);
            textMeshPro.text = "";
        }
    }

    IEnumerator TypeSentence(string sentence)
    {
        textMeshPro.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            textMeshPro.text += letter;
            yield return new WaitForSeconds(delayBetweenCharacters);
        }
    }
}
