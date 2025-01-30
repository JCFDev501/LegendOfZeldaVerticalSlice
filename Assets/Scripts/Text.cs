using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// Example of how to use show text
// StartCoroutine(ShowText("Stuff you want it to show");

public class Text : MonoBehaviour
{
    [SerializeField] private float m_delay = 0.1f;
    [SerializeField] private GameObject m_player;

    private string m_currentText;

    public IEnumerator ShowText(string text)
    {
        PlayerController controller = m_player.GetComponent<PlayerController>();

        for(int i = 0; i < text.Length; i++)
        {
            controller.SetControllable(false);

            m_currentText = text.Substring(0, i);
            GetComponent<TMP_Text>().text = m_currentText;
            yield return new WaitForSeconds(m_delay);

            controller.SetControllable(true);
        }
    }

    public void ClearText()
    {
        GetComponent<TMP_Text>().text = "";
    }
}
