using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GoogleCloudStreamingSpeechToText;
using System.Linq;
using System.Text;
using UnityEngine.SceneManagement;
//using static GameplayControl;

public class Change_text_continuous : MonoBehaviour
{
    private bool sw;
    //private bool press;
    //private float last_input;
    private int question_num=0;
    private string[] question_text={
        "พูด 10",
        "พูด 9",
        "พูด 8",
        "พูด 7",
        "พูด 6",
        "พูด 5",
        "พูด 4",
        "พูด 3",
        "พูด 2",
        "พูด 1"
    };
    private string[] answer_text={
        "10",
        "9",
        "8",
        "7",
        "6",
        "5",
        "4",
        "3",
        "2",
        "1"
    };
    
    //public bool[] result;

    public Text MyText;
    
    public Text[] Transcript;

    public Text Question;

    StreamingRecognizer sr;

    Coroutine route;

    public GameplayControl gameplayControl;
    
    //public  gameplayControl = this.GetComponent<GameplayControl>();

    private void Awake()
    {
        GameObject gameobject = new GameObject("StreamingRecognizer");
        sr = gameObject.AddComponent<StreamingRecognizer>();
        sr.startOnAwake = false;
    }

    void Start()
    {
        //Text sets your text to say this message
        MyText.text = "Just start!";
        for(int i = 0;i<10;i++){
            Transcript[i].text = "Result: ";
        }
        sr.enableDebugLogging = true;
        sw = true;
        //press = false;
        Question.text = question_text[question_num];
        gameplayControl.UpdateScoreText();
    }

    void Update()
    {
        //Press the space key to change the Text message
        //string name "Jump" can be find in edit -> project setting -> input manager
        if (Input.GetKeyDown("space"))
        {
            if(sw == false){
                MyText.text = "Stop listening";
                sr.StopListening();
                sr.onInterimResult.RemoveListener(ListenInterim);
                sr.onFinalResult.RemoveListener(ListenFinal);
                sw = true; 
                gameplayControl.delay = 0;
                gameplayControl.kill = true;
            }
            else { 
                MyText.text = "Start listening";
                sr.StartListening();
                sr.onInterimResult.AddListener(ListenInterim);
                sr.onFinalResult.AddListener(ListenFinal);
                sw = false;
                gameplayControl.kill = false;
                gameplayControl.delay = 600;
                gameplayControl.CallCountdown();
            }
        }
    }

    void ListenInterim(string i){
        if(i == answer_text[question_num]){
            if(gameplayControl.InTime){
                CorrectInTime();
            }
            else{
                CorrectTimeUp();
            }
            AdjustWhenCheckAnswer();
        }
    }

    void ListenFinal(string i)
    {
        if(i == "ออก"){
            SceneManager.LoadScene("Main menu");
        }
        if(i.All(l => char.IsDigit(l) || ',' == l|| '.' == l || '-' == l))
        {
            if(i == answer_text[question_num]){
                if(gameplayControl.InTime){
                    CorrectInTime();
                }
                else{
                    CorrectTimeUp();
                }
            }
            else{
                Incorrect();
            }
            AdjustWhenCheckAnswer();
        }
    }

    void AdjustWhenCheckAnswer(){
        sr.StopListening();
        sr.onInterimResult.RemoveListener(ListenInterim);
        sr.onFinalResult.RemoveListener(ListenFinal);
        question_num++;
        Question.text = question_text[question_num];
        sr.StartListening();
        sr.onInterimResult.AddListener(ListenInterim);
        sr.onFinalResult.AddListener(ListenFinal);
        gameplayControl.delay = 600;
        if(gameplayControl.kill){
            gameplayControl.kill = false;
            gameplayControl.CallCountdown();
        }
    }

    void CorrectInTime(){
        Transcript[question_num].text = "Result:  ✓";
        Debug.Log("correct");
        gameplayControl.AnswerCorrect();
    }

    void CorrectTimeUp(){
        Transcript[question_num].text = "Result:  ✓ but time's up";
        Debug.Log("correct but time's up");
        gameplayControl.AnswerCorrectButTimeUp();
    }

    void Incorrect(){
        Transcript[question_num].text = "Result:  X";
        Debug.Log("incorrect");
        gameplayControl.AnswerWrong();
    }
}