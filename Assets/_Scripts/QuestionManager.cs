using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestionManager : MonoBehaviour {


    private string answerRaw = "Яблоко";
    private string variantsRaw = "Слива,Груша,Яблоко,Дыня";

    

    public Text[] buttonTexts;
    public Color leNormal;
    public Color leGreen;
    public Color leRed;


    public string[] variants;

	// Use this for initialization
	void Start () {
        variants = variantsRaw.Split(',');

        for (int i = 0; i < buttonTexts.Length; i++)
        {
            buttonTexts[i].text = variants[i];
            buttonTexts[i].transform.parent.GetComponent<Image>().color = leNormal;
        }
        
	}
	
	// Update is called once per frame
	void Update () {
        
	}


    //check for clicks and answer
    public void CheckAnswer(GameObject button)
    {
        if(button.transform.GetChild(0).GetComponent<Text>().text == answerRaw)
        {
            button.GetComponent<Image>().color = leGreen;
        }
        else
        {
            button.GetComponent<Image>().color = leRed;
        }
    }
}
