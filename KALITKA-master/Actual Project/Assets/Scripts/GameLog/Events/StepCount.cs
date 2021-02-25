using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Скрипт подсчета количества оставшихся ходов.
// Прикреплен к тексту с отображением оставшихся ходов.
public class StepCount : MonoBehaviour
{
    void Update()
    {
        // Если ходов не осталось -- выводим сообщение о том, что нужно бросить кубик 
        if(MapGenerator.turn_count == 0){
            if(!MapGenerator.game_is_over)
                GetComponent<Text>().text = "Бросьте кубик";
            else{
                GetComponent<Text>().text = "Игра окончена";
            }
        }
        // Иначе отображаем количество оставшихся ходов
        else{
            if(!MapGenerator.game_is_over)
                GetComponent<Text>().text = "Осталось ходов: " + MapGenerator.turn_count;
            else{
                GetComponent<Text>().text = "Игра окончена";
            }
        }
    }
}