using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;
    // Это скрипт генерации игровой карты.
    // Запускается вместе со сценой GameScene
public class TutorialMapGenerator : MonoBehaviour
{
    public static bool turn = false; // Информация о том, чей ход. 
    // false -- синего
    // true -- красного
    public static int turn_count = 0;// Информация о том сколько ходов осталось сделать 

    public static int n = 3, m = n; // Это размеры карты, к которым иногда нужно обращаться
        // Изначальный размер карты 6х6, но в настройках есть поле, с помощью которого размеры можно поменять

    public static float block_lenght = 3.5388f, r = 0.2f;
        // block_lenght - это длина блока (нужно для генерации карты)
        // r - расстояние между блоками
    
    public struct Square // Структура Square - то есть единица игрового поля (у нас это кубы).
    {
        public int level, owner;
            // level - это уровень куба
            // owner - обладатель куба (-1 - у блока нет владельца, 0 - владелец синий, 1 - владелец красный)
    }
    public struct Pair{
        public int f, s;
        public Pair(int x, int y){
            f = x;
            s = y; 
        }
    }
    public GameObject block; // Сюда пихаем модель блока.

    public static Square[,] map = new Square[n, m]; // Матрица блоков -- то есть сама карта, в которой хранится информация о каждом блоке.
        // У блоков есть свой индекс, которых хранится в переменных a, b в самих блоках.
        // Если что, матрица здесь -- это двумерный массив.
    public static Pair[] tutorial_moves = {
        new Pair(1, 0),
        new Pair(1, 2),
        new Pair(1, 2),
        new Pair(1, 0),
        new Pair(2, 0),
        new Pair(1, 2),
        new Pair(1, 0),
        new Pair(1, 1),
        new Pair(1, 1),
        new Pair(1, 1),
        new Pair(1, 2),
        new Pair(2, 2)
    };
    public static int[] dice_count = {1, 2, 2, 1, 2, 1, 3};
    public static int tutorial_move_number = 0;
    public static int tutorial_count = 0;
    public static int[] players = {1, 1}; // Массив, в котором хранятся значения текущих очков. [0] - синего,  [1] - красного 
    public static bool game_is_over = false;
    void Start()
    {
        // Переназначаем переменные для распределенного рандома (на тот случай, если настройки поменяются).
        game_is_over = false;
        // Это нужно для того, чтобы при каждой новой загрузке карты сбрасывались значения ходов. 
        turn = false;
        turn_count = 0;
        tutorial_move_number = 0;
        tutorial_count = 0;
        // Изначально у каждого игрока по 1 очку, так как начальные клетки -- это пустые блоки.
        players[0] = 1;
        players[1] = 1;
        for(int i = 0; i < n; i++){ // Цикл прохода по "строкам" или по условной оси Ох.
            for(int j = 0; j < m; j++){ // Цикл прохода по "столбцам" или по условной оси Оу.
                    // Здесь мы n раз проходим по всем j от 0 до m, тем самым проходим каждый квадрат карты. (Если кому-то покажется сложным
                block.GetComponent<TutorialSquareChanger>().a = i; // Присваиваем блоку первый индекс (по оси Ох или по "строкам")
                block.GetComponent<TutorialSquareChanger>().b = j; // Присваем блоку второй индекс (по оси Оу или по "столбцам")
                //     // Выгружаем этот игровой объект на карту с координатами соответствующими его индексу в матрице.
                    // Здесь учитывается размер блока и расстояние между клетками.
                Instantiate(block, new Vector3((float)i * (block_lenght + r), 0, (float)j * (block_lenght + r)), Quaternion.identity);
                map[i, j].owner = -1; // Присваем owner -1 для того чтобы блок понимал, что он ничейный.
                // //Debug.Log(dist);
                map[i, j].level =  0;
            }
        }

        map[0,0].owner = 0; // В блоке с индексом 0,0 ставим принадлежность синего игрока (то есть это теперь блок синего).
        map[n-1, m-1].owner = 1; // В последнем блоке матрицы ставим принадлежность красного.
        map[1, 1].level = 2;
        map[0, 2].level = 1;
        map[2, 0].level = 1;
    }
    void Update(){
        if(turn && turn_count > 0){
            Thread.Sleep(1000);
             if(tutorial_move_number >= tutorial_moves.Length){
                return;
            }
            if(map[tutorial_moves[tutorial_move_number].f , tutorial_moves[tutorial_move_number].s].owner == 1){
                map[tutorial_moves[tutorial_move_number].f , tutorial_moves[tutorial_move_number].s].level++;
                players[1] ++;
            }
            else{
                players[1] += map[tutorial_moves[tutorial_move_number].f , tutorial_moves[tutorial_move_number].s].level + 1;
                if(map[tutorial_moves[tutorial_move_number].f , tutorial_moves[tutorial_move_number].s].owner == 0)
                    players[0] -= (map[tutorial_moves[tutorial_move_number].f , tutorial_moves[tutorial_move_number].s].level + 1);
                map[tutorial_moves[tutorial_move_number].f , tutorial_moves[tutorial_move_number].s].owner = 1;
            }
            turn_count--;
            tutorial_move_number++;
            if(turn_count == 0) // Если это был последний ход (turn_count == 0)
                turn = !turn; 
        }
    }
}