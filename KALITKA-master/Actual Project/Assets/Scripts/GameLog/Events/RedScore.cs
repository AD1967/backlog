using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RedScore : MonoBehaviour
{
    void Update(){
 		GetComponent<Text>().text = "" + MapGenerator.players[1]; 
    }
}