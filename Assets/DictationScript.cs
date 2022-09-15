using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows.Speech;

public class DictationScript : MonoBehaviour
{
    [SerializeField]
    private Text m_Hypotheses;

    [SerializeField]
    private Text m_Recognitions;

    private DictationRecognizer m_DictationRecognizer;
    
    void Start()
    {
        m_DictationRecognizer = new DictationRecognizer();
        m_DictationRecognizer.AutoSilenceTimeoutSeconds = 60.0F;
        m_DictationRecognizer.InitialSilenceTimeoutSeconds = 60.0F;

        m_DictationRecognizer.DictationResult += (text, confidence) =>
        {
            Debug.LogFormat("Dictation result: {0}", text);
            //m_Recognitions.text += text + "\n";
        };

        m_DictationRecognizer.DictationHypothesis += (text) =>
        {
            Debug.LogFormat("Dictation hypothesis: {0}", text);
            //m_Hypotheses.text += text;
        };

        m_DictationRecognizer.DictationComplete += (completionCause) =>
        {
            if (completionCause != DictationCompletionCause.Complete)
                Debug.LogErrorFormat("Dictation completed unsuccessfully: {0}.", completionCause);
        };

        m_DictationRecognizer.DictationError += (error, hresult) =>
        {
            Debug.LogErrorFormat("Dictation error: {0}; HResult = {1}.", error, hresult);
        };

        m_DictationRecognizer.Start();
    }

    void Update()
    {
        //Press the space key to change the Text message
        if (Input.GetKeyDown("space"))
        {
            m_DictationRecognizer.Stop();
        }
        if (Input.GetKeyDown("enter"))
        {
            m_DictationRecognizer.Start();
        }
        if (Input.GetKeyDown("s"))
        {
            Debug.LogFormat("Dictation result: {0}", m_DictationRecognizer.Status);
        }
        if (Input.GetKeyDown("d"))
        {
            m_DictationRecognizer.Dispose();
        }
    }
}