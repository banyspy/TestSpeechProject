using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Text;

public class GameplayControl : MonoBehaviour{
    //public int delay;
    public  bool InTime = true;
    public  bool kill = false;
    public  int delay = 600;
    public Text TimeLeft;
    public Text ScoreText;
    public int score = 0;
    private int MilliSecond;
    private int Second;

    public void CallCountdown(){
        StartCoroutine(Countdown());
    }

    public void CallStopCountdown(){
        StopCoroutine(Countdown());
    }

    public void AnswerCorrect(){
        score += 10;
        UpdateScoreText();
    }

    public void AnswerCorrectButTimeUp(){
        score += 1;
        UpdateScoreText();
    }

    public void AnswerWrong(){
        score += 0;
        UpdateScoreText();
    }

    public void UpdateScoreText(){
        Debug.Log("Update score!");
        ScoreText.text = string.Format("Score: {0}",score);
    }

    private IEnumerator Countdown(){
        while(true){
            Debug.Log("Start the loop!");
            if(kill) yield break;
            InTime = true;
            //delay/=10;
            while(delay>0 && !(kill)){
                Second = Mathf.FloorToInt(delay/100);
                MilliSecond = Mathf.FloorToInt(delay%100);
                //Debug.Log(string.Format("Time Left: {0:000}:{1:000}",Second ,MilliSecond));
                TimeLeft.text = string.Format("Time Left: {0:00}:{1:00}",Second ,MilliSecond);
                yield return new WaitForSeconds(0.01f);
                delay-=1;
            }
            InTime = false;
            if(!kill)TimeLeft.text = ("Time's UP!!");
            if(!kill)Debug.Log("Time's up!!'");
            kill = true;
            yield break;
        }
    }
}