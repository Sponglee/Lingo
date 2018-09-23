using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Security.Cryptography;
//using System;

public class QuestionManager : MonoBehaviour {

    private string variantsRaw = "Абрикос,Ананас,Апельсин,Арбуз,Авокадо,Банан,Барбарис,Черника,Дыня,Ежевика,Фейхоа,Гранат,Грейпфрут,Груша,Хурма,"+
    "Инжир,Киви,Клубника,Крыжовник,Лайм,Личи,Лимон,Малина,Мандарин,Манго,Маракуя,Облепиха,Папая,Персик,Памела," +
    "Шиповник,Слива,Смородина,Виноград,Вишня,Яблоко,Земляника";

   

    public Sprite[] images;
    public int answerIndex;
    public Slider slider;

    public Image questionImage;
    public Text[] buttonTexts;
    public Color leNormal;
    public Color leGreen;
    public Color leRed;

    //number of questions per round
    public int questionCount = 15;
    public int currentQuestion;
    //for generating random set of questions
    public List<int> answerIndVarArray;

    //for keeping question set from prefs                                  ************NOTE - CLEAR THIS ON EXIT TO MENU
    public string[] questionsRaw;
    public string[] answersRaw;

    // Use this for initialization
    void Start()
    {
        

        //Divide string of answers
        answersRaw = variantsRaw.Split(',');

        //grab current question
        currentQuestion = PlayerPrefs.GetInt("CurrentQuestion", 0);

        //If questions not selected - generate a list of random ones
        if (PlayerPrefs.GetString("AnswerIndVars", "") == "")
        {
            //if there's none - form a string of random questions
            string answerIndVars = "";
            //Grag unique answer per session from pool
            for (int i = 0; i < questionCount; i++)
            {
                int index;

                do
                {
                    index = Random.Range(0, answersRaw.Length);
                }
                while (answerIndVarArray.Contains(index));
                answerIndVarArray.Add(index);
                answerIndVars += index.ToString();
                if (i < questionCount - 1)
                {
                    answerIndVars += ",";
                }
            }

            PlayerPrefs.SetString("AnswerIndVars", answerIndVars);
        }
       
        //grab index and make array of questions from prefs
        string questionsPref = PlayerPrefs.GetString("AnswerIndVars", "");
        //Divide an array from string of questions
        questionsRaw = questionsPref.Split(',');
        //Grab currentQuestion for answerIndex
        answerIndex = int.Parse(questionsRaw[PlayerPrefs.GetInt("CurrentQuestion", 0)]);
        PlayerPrefs.SetInt("AnswerIndex", answerIndex);
            
     

        answerIndex = PlayerPrefs.GetInt("AnswerIndex", 0);




        questionImage.sprite = images[answerIndex];


        //adjust slider value to correspond
        slider.value = (float)currentQuestion/(float)questionCount;

        //Random button with answer position
        int answerPosition = Random.Range(0, 4);

        //Remember variants to check for repeats
        List<int> buttonIndVars = new List<int>();

        for (int i = 0; i < buttonTexts.Length; i++)
        {
            if(i == answerPosition)
            {
                buttonTexts[i].text = answersRaw[answerIndex];
                continue;
            }

            //grab random word
            int buttonIndex=0;
            // make sure it's not answer twice or is already taken or out of range
            do
            {
                buttonIndex += Random.Range(0, answersRaw.Length);
                //Get new one from 0 if it's out of range
                if (buttonIndex > answersRaw.Length)
                    buttonIndex = Random.Range(0, answersRaw.Length);
            }
            while (buttonIndex == answerIndex || buttonIndVars.Contains(buttonIndex));
                //remember index
                buttonIndVars.Add(buttonIndex);

            //update the button
            buttonTexts[i].text = answersRaw[buttonIndex];
            buttonTexts[i].transform.parent.GetComponent<Image>().color = leNormal;
        }


        //Clear memory
        buttonIndVars.Clear();
	}
	
	// Update is called once per frame
	void Update () {
        
	}


    //check for clicks and answer
    public void CheckAnswer(GameObject button)
    {
        //Answer Is correct - load new set
        if(button.transform.GetChild(0).GetComponent<Text>().text == answersRaw[answerIndex])
        {
           
            if (currentQuestion < questionCount-1)
            {
                button.GetComponent<Image>().color = leGreen;
                currentQuestion = PlayerPrefs.GetInt("CurrentQuestion", 0);
                answerIndex = int.Parse(questionsRaw[currentQuestion + 1]);
                PlayerPrefs.SetInt("AnswerIndex", answerIndex);
                //iterate question further
                PlayerPrefs.SetInt("CurrentQuestion", currentQuestion + 1);

                SceneManager.LoadScene("ImageGuess");
            }
            else
            {
                slider.value = 1;
                Debug.Log("U WIN!");
            }
        }
        else
        {
            button.GetComponent<Image>().color = leRed;
        }
    }

    

    public void ResetAll()
    {
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene("ImageGuess");
    }
}





