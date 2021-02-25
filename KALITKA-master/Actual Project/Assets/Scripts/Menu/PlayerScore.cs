using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScore : MonoBehaviour
{	
	public static string player_score = "my_points", beginner = "beginner";
	public static int score;
	public static int new_player; 
	void Start(){
		
		if(!PlayerPrefs.HasKey(player_score))
			PlayerPrefs.SetInt(player_score, 0);
		score = PlayerPrefs.GetInt(player_score);

		if(!PlayerPrefs.HasKey(beginner))
			PlayerPrefs.SetInt(beginner, 0);
		new_player = PlayerPrefs.GetInt(beginner);
    } 
    void Update(){
    	GetComponent<Text>().text = score + "";
    }
}
