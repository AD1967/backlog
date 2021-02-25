using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialBlueScore : MonoBehaviour
{
    void Update(){
 		GetComponent<Text>().text = "" + TutorialMapGenerator.players[0]; 
    }
}
