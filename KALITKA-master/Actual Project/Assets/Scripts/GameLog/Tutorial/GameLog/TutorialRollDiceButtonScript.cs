using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Скрипт для кнопки броска кубиков.
// Прикреплен к кнопке, соответственно,
// работает с кнопкой
public class TutorialRollDiceButtonScript : MonoBehaviour
{
    public AudioClip RollAudio;
    void Update(){
        if(TutorialMapGenerator.turn && TutorialMapGenerator.turn_count == 0 && TutorialMapGenerator.players[0] > 0 && TutorialMapGenerator.players[1] > 0){
            RollDice();
        }
        // Делаем кнопку недоступной если пользователь ещё может сходить (MapGenerator.turn_count > 0;
        GetComponent<Button>().interactable = (TutorialMapGenerator.turn_count == 0 && TutorialMapGenerator.players[0] > 0 && TutorialMapGenerator.players[1] > 0);
    }
    ///<summary>
    /// Функция броска кубиков. 
    /// При срабатывании меняет количество ходов пользователя
    /// от 1 до 3 (включительно).
    ///</summary>
    public void RollDice(){
        //Присваиваем переменной количества ходов рандомное значение от 1 до 3 включительно.
        PlaySound(RollAudio);
        TutorialMapGenerator.turn_count = TutorialMapGenerator.dice_count[TutorialMapGenerator.tutorial_count];
        TutorialMapGenerator.tutorial_count ++;
         if( TutorialMapGenerator.tutorial_count >= TutorialMapGenerator.dice_count.Length){
                return;
        }
    }
    private void PlaySound(AudioClip sound)
    {
        //Объявляем компонент на объекте скрипта
        var Source = GetComponent<AudioSource>();
        //Меняем на нужный нам звук
        Source.clip = sound;
        //Меняем звук в зависимости на которые в настройках игры
        Source.volume = GameSettings.GameVolume;
        //Проигрываем звук
        Source.Play();
    }
}
