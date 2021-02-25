using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlueScore : MonoBehaviour
{
    void Update(){
 		GetComponent<Text>().text = "" + MapGenerator.players[0]; 
    }
}
