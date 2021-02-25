using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Скрипт подсчета количества оставшихся ходов.
// Прикреплен к тексту с отображением оставшихся ходов.
public class TutorialStepCount : MonoBehaviour
{
    void Update()
    {
        // Если ходов не осталось -- выводим сообщение о том, что нужно бросить кубик 
        if(TutorialMapGenerator.turn_count == 0){
            if(!TutorialMapGenerator.game_is_over)
                GetComponent<Text>().text = "Бросьте кубик";
            else{
                PlayerScore.new_player = 1;
                PlayerPrefs.SetInt("beginner", PlayerScore.new_player);
                GetComponent<Text>().text = "Игра окончена";
            }
        }
        // Иначе отображаем количество оставшихся ходов
        else{
            if(!TutorialMapGenerator.game_is_over)
                GetComponent<Text>().text = "Осталось ходов: " + TutorialMapGenerator.turn_count;
            else{
                PlayerScore.new_player = 1;
                PlayerPrefs.SetInt("beginner", PlayerScore.new_player);
                GetComponent<Text>().text = "Игра окончена";
            }
        }
    }
}