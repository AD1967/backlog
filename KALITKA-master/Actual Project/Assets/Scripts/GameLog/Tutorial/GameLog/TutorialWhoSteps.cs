using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Скрипт вывода информации о том, кто ходит.
// Прикреплен к соответствующему тексту
public class TutorialWhoSteps : MonoBehaviour
{
    void Update()
    {
    	var text = GetComponent<Text>();
    
    	if(TutorialMapGenerator.players[0] == 0){
            text.text = "Красный Победил!!!";
            TutorialMapGenerator.game_is_over = true;
            return;
        }
        
        if(TutorialMapGenerator.players[1] == 0){
            text.text = "Синий Победил!!!";
            TutorialMapGenerator.game_is_over = true;
            return;
        }
        // Если turn == false выводим что ходит синий
        // Если true -- ходит красный
        text.text = (TutorialMapGenerator.turn ? "Ходит красный" : "Ходит синий");
    }
}
