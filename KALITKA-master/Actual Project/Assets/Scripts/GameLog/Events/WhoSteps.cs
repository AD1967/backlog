using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Скрипт вывода информации о том, кто ходит.
// Прикреплен к соответствующему тексту
public class WhoSteps : MonoBehaviour
{
    void Update()
    {
    	var text = GetComponent<Text>();
    
    	if(MapGenerator.players[0] == 0){
            text.text = "Красный Победил!!!";
            MapGenerator.game_is_over = true;
            return;
        }
        
        if(MapGenerator.players[1] == 0){
            text.text = "Синий Победил!!!";
            if(!MapGenerator.game_is_over){
                PlayerScore.score += MapGenerator.n * 5;
                PlayerPrefs.SetInt(PlayerScore.player_score, PlayerScore.score);
            }
            MapGenerator.game_is_over = true;
            return;
        }
        // Если turn == false выводим что ходит синий
        // Если true -- ходит красный
        text.text = (MapGenerator.turn ? "Ходит красный" : "Ходит синий");
    }
}
