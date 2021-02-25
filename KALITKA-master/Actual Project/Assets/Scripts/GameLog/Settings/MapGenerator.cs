using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

    // Это скрипт генерации игровой карты.
    // Запускается вместе со сценой GameScene
public class MapGenerator : MonoBehaviour
{
    public static int type = 6; // тип карты: 4 - квадрат, 6 - гексагон, 8 - октагон

    public static bool turn = false; // Информация о том, чей ход. 
    // false -- синего
    // true -- красного
    public static int turn_count = 0;// Информация о том сколько ходов осталось сделать 

    public static int n = 5, m = n; // Это размеры карты, к которым иногда нужно обращаться
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
    public GameObject octablock, quadroblock; // Сюда пихаем модель блока.
    private GameObject block;

    public static Square[,] map = new Square[n, m]; // Матрица блоков -- то есть сама карта, в которой хранится информация о каждом блоке.
        // У блоков есть свой индекс, которых хранится в переменных a, b в самих блоках.
        // Если что, матрица здесь -- это двумерный массив.
    public static int[] players = {1, 1}; // Массив, в котором хранятся значения текущих очков. [0] - синего,  [1] - красного 
    public static bool game_is_over = false;
    // Это пошли переменные, необходимые для генерации карты с распределенным рандомом.
    private static int max_dist = Math.Max(n, m); // Максимальное расстояние до клетки
    private static int max_level = 4; // Изначально на карте спаунятся только 4 дома.
    private static double[] max_func = new double[max_level + 1]; // Точки, в которых будут максимумы для уровней.
    private static double[,] level_dist_r = new double[max_level + 1, max_dist]; // По строчкам уровень, а по столбцам вероятностное распределение для растояния i.
    private static double[] sum = new double[max_dist]; // Сумма коэфициентов для каждой дистанции

    void Start()
    {
        switch(type){
            case 4: 
                block = quadroblock;
                break;
            case 8:
                block = octablock;
                break;
        }
        // Переназначаем переменные для распределенного рандома (на тот случай, если настройки поменяются).
        game_is_over = false;
        max_dist = Math.Max(n, m);
        level_dist_r = new double[max_level + 1, max_dist];
        sum = new double[max_dist];

        // Это нужно для того, чтобы при каждой новой загрузке карты сбрасывались значения ходов. 
        turn = false;
        turn_count = 0;

        // Изначально у каждого игрока по 1 очку, так как начальные клетки -- это пустые блоки.
        players[0] = 1;
        players[1] = 1;
        
        m = n;
        map = new Square[n, m];
        
        if(type == 6) 
            return;
        // Опять же, чтобы соответствовать настройкам.
        


        for(int i = 0; i <= max_level; i++) // Расчет точки максимума (или аргумента) функции для всех возможных уровней.
            max_func[i] = (double)i * ((double)max_dist / (double)max_level); 

        for(int i = 0; i <= max_level; i++) // Расчет значений функций для всех возможных уровней и расстояний.
            for(int j = 0; j < max_dist; j++){
                level_dist_r[i,j] = 1 / Math.Pow(Math.Abs((double)j - max_func[i]), 1.5);
                sum[j] += level_dist_r[i, j];
            }
            
        
        
        for(int i = 0; i < n; i++){ // Цикл прохода по "строкам" или по условной оси Ох.
            for(int j = 0; j < m; j++){ // Цикл прохода по "столбцам" или по условной оси Оу.
                    // Здесь мы n раз проходим по всем j от 0 до m, тем самым проходим каждый квадрат карты. (Если кому-то покажется сложным)

                int dist = Math.Min(i + j, n - 1 - i + m - 1 - j) - 1; // Минимальное расстояние до начальной клетки

                block.GetComponent<SquareChanger>().a = i; // Присваиваем блоку первый индекс (по оси Ох или по "строкам")
                block.GetComponent<SquareChanger>().b = j; // Присваем блоку второй индекс (по оси Оу или по "столбцам")

                    // Выгружаем этот игровой объект на карту с координатами соответствующими его индексу в матрице.
                    // Здесь учитывается размер блока и расстояние между клетками.
                var rotate = block.transform.rotation;
                if(type == 8)
                    rotate.y = 0.20f;
                Instantiate(block, new Vector3((float)i * (block_lenght + r), 0, (float)j * (block_lenght + r)), rotate);

                map[i, j].owner = -1; // Присваем owner -1 для того чтобы блок понимал, что он ничейный.
                // //Debug.Log(dist);
                map[i, j].level = (dist == -1 ? 0 : choiseLevel(dist)); // Если клетка не начальная, то рандомим для неё уровень
            }
        }

        map[0,0].owner = 0; // В блоке с индексом 0,0 ставим принадлежность синего игрока (то есть это теперь блок синего).
        map[n-1, m-1].owner = 1; // В последнем блоке матрицы ставим принадлежность красного.
        map[0, 0].level = 0;
        map[n-1, m-1].level = 0;
    }

    static System.Random random  = new System.Random(); // Создаем экземпляр рандома.
    public int choiseLevel(int dist){
        double rand = random.NextDouble() * sum[dist]; // Hаходим рандомное нецелочисленное число от 0 до sum[dist]
        //Debug.Log(rand);
        int level = 0; 
        double s = 0; // Объявляем переменную к которой будем прибавлять значения level_dist_r чтобы понять на каком промежутке находится рандом
        while(level <= max_level && s < rand){ // пока не достигли максимального уровня и эта самая s меньше рандомного числа.
            s+= level_dist_r[level, dist]; 
            level++;
        }
        return level - 1; // Выводим уровень - 1 
    }
}