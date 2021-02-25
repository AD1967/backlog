using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

// Скрипт, который отрабатывает по клику на какую-либо клетку.
// Лежит в каждом блоке.

public class TutorialSquareChanger : MonoBehaviour
{
    public int a, b; // Кординаты данной клетки в матрице. a - по оси Ох, b - по Оу
    private int cur_level = 0;
    public static int max_level = 4; // Текущий уровень блока и максимальный уровень.
    public GameObject house; // Сюда суем модельку дома.
    public GameObject townhall; // Сюда модельку ратуши. 
    public Material red_block, blue_block, red_house, blue_house; // Сюда все необходимые текстуры
    private Vector3 start_pos, target;
    private float speed = 0.5f;
    public static Vector3[] HouseMarkers = // Расположения домиков относительно центра блока.
    {
        new Vector3(-0.2420002f, 0.5538601f, 0.0960001f),
        new Vector3(0.271f, 0.5538601f, 0.13f),
        new Vector3(-0.241f, 0.5538601f, -0.348f ),
        new Vector3(0.271f, 0.5538601f, -0.312f)
    };

    void Start(){ // На старте грейдим блок в соответствии с поставленным на него уровнем (ставим ему домики)
        start_pos = gameObject.transform.position;
        target = start_pos + new Vector3(0, 0.3f, 0);
       squareUpgrade();
    }

    void Update(){
        if(TutorialMapGenerator.tutorial_move_number < TutorialMapGenerator.tutorial_moves.Length){
            if(TutorialMapGenerator.tutorial_moves[TutorialMapGenerator.tutorial_move_number].f == a && TutorialMapGenerator.tutorial_moves[TutorialMapGenerator.tutorial_move_number].s == b && !TutorialMapGenerator.turn){
                transform.position = Vector3.MoveTowards (transform.position, target, Time.deltaTime * speed);
		        if (transform.position == target && target.y != start_pos.y)
			        target.y = start_pos.y;
		        else 
                    if (transform.position == target && target.y == start_pos.y)
			            target.y = start_pos.y + 0.3f;
            }else{
                gameObject.transform.position = start_pos;
            }
        }else{
            gameObject.transform.position = start_pos;
        }
        if(TutorialMapGenerator.map[a,b].owner != -1){ // Смотрим, принадлежит ли данный блок какому-либо игроку.
            paintSquare(TutorialMapGenerator.map[a,b].owner); // Если принадлежит выполняем функцию, которая перекрашивает блок под игрока
        }

        if(cur_level < TutorialMapGenerator.map[a,b].level) // Если уровень увеличился 
            squareUpgrade(); // Грейдим блок.
    }

    /// <summary>
    /// Функция, которая апгрейдит блок,
    /// то есть ставит на него домики в соответствии с текущим уровнем.
    /// </summary>
    void squareUpgrade(){
        if(TutorialMapGenerator.map[a, b].level >= 1 && TutorialMapGenerator.map[a, b].level <= 4){ // Если уровень блока предполагает, что у него только домики. т.е. от 1 до 4 включительно
            while(cur_level < TutorialMapGenerator.map[a, b].level){ // Пока текущий уровень блока меньше его настоящего уровня, ставим дома.
                    // //Debug.Log("UGRADE" + a + " " + b);
                    GameObject cur_house = Instantiate(house, transform); // Создаем дом.
                    cur_house.transform.localScale = new Vector3 (1.0f / TutorialMapGenerator.block_lenght, 1.0f / TutorialMapGenerator.block_lenght, 1.0f / TutorialMapGenerator.block_lenght);
                        // Даем ему размеры (иначе дома спаунятся размером с блок)
                    cur_house.transform.localPosition = HouseMarkers[cur_level]; // Перемещаем в соответствии с их заранее прописанными координатами.
                    cur_level++; // Увеличиваем текущий уровень блока.
            }
        }
    }
 
    /// <summary> 
    /// Это функция перекраски клетки под того или иного игрока.
    /// </summary>
    void paintSquare(int who){
        if(who == 1) // Если блок красного игрока
            painter(red_house, red_block);
        if(who == 0) // Если блок синего игрока
            painter(blue_house, blue_block);
    }

    ///<summary> 
    /// Эта функция всё что делает - это меняет материалы домов и блока.
    ///</summary>
    void painter(Material color_of_house, Material color_of_block){ // Материал дома, материал блока.
        gameObject.GetComponent<Renderer>().material = color_of_block; // Меняем материал блока.
        for(int i = 0; i < gameObject.transform.childCount; i++) // Проходимя по детям.
            transform.GetChild(i).GetChild(0).GetComponent<Renderer>().material = color_of_house;  // Меняем материал.
    }


    /// <summary> 
    /// Метод, который срабатывает, если на объект кликнули мышью.
    /// Перед релизом необходимо будет переделать на тапы, чтобы было совместимо с андроид девайсами.
    /// </summary>
    void OnMouseDown()
    {
        if(TutorialMapGenerator.players[0] == 0 || TutorialMapGenerator.players[1] == 0){ // Если у кого-то больше нет очков.
            //Debug.Log("Перезапустите игру");
            return; // Не реагируем, пушто закончилась игра уже всё.
        }
        if(GameSettings.GameWithBot && TutorialMapGenerator.turn)
            return;

        if(TutorialMapGenerator.turn_count == 0) // Если кликнули, когда ходов не осталось (turn_count == 0)
            return; // просто выходим из метода, так как перекраска невозможна
         if(TutorialMapGenerator.tutorial_move_number > TutorialMapGenerator.tutorial_moves.Length)
            return;
        if(!TutorialMapGenerator.turn && (TutorialMapGenerator.tutorial_moves[TutorialMapGenerator.tutorial_move_number].f != a || TutorialMapGenerator.tutorial_moves[TutorialMapGenerator.tutorial_move_number].s != b))
            return;
        if(TutorialMapGenerator.turn_count > 0) // Если ходить всё-таки можно (turn_count > 0)
            changeSquare(); // Выполняем метод changeSquare (меняем клетку)
        TutorialMapGenerator.tutorial_move_number ++;
        if(TutorialMapGenerator.turn_count == 0) // Если это был последний ход (turn_count == 0)
            TutorialMapGenerator.turn = !TutorialMapGenerator.turn; // Ход переходит к другому игроку.
    }

    /// <summary> 
    /// Функция, которая меняет блок.
    /// </summary>
    void changeSquare()
    {   
        if(TutorialMapGenerator.map[a, b].owner == (TutorialMapGenerator.turn ? 1 : 0)){   // Если клетка наша, то апгрейдим.
            if(cur_level == max_level){ // Если уровень максимальный -- выходим из апгрейда.
                //Debug.Log("У клетки максимальный уровень.");
                return;
            }
            // Иначе -- увеличесваем уровень.
            TutorialMapGenerator.map[a, b].level++;
            TutorialMapGenerator.players[(TutorialMapGenerator.turn ? 1 : 0)]++;
            TutorialMapGenerator.turn_count--;
        }
        else // Если клетка не наша, то захватываем.
            killTheOpponent(TutorialMapGenerator.map[a, b].level + 1, (TutorialMapGenerator.map[a, b].owner >= 0 ? true : false));
    }
 
    /// <summary> 
    /// Функция, которая захватывает данный блок.
    /// </summary>

    void killTheOpponent(int need_to_beat, bool opponent_is_real) // Минимальное количество очков, которое необходимо для захвата; Нейтральный блок или блок протвника.
    {
        int MX = -1; // Максимальное количество наших очков на соседних клетках
        for(int i = -1; i <= 1; i += 2){ // Проходимся по соседним "своим" клеткам и находим ту, у которой максимальный уровень
            if(a + i >= 0 && a + i < TutorialMapGenerator.n && TutorialMapGenerator.map[a + i, b].owner == (TutorialMapGenerator.turn ? 1 : 0))
                MX = Math.Max(MX, TutorialMapGenerator.map[a + i, b].level + 1);
            if(b + i >= 0 && b + i < TutorialMapGenerator.m && TutorialMapGenerator.map[a, b + i].owner == (TutorialMapGenerator.turn ? 1 : 0))
                MX = Math.Max(MX, TutorialMapGenerator.map[a, b + i].level + 1);
        }

        if(MX >= need_to_beat){ // Если мы убить этот блок можем, то захватываем
            TutorialMapGenerator.players[(TutorialMapGenerator.turn ? 1 : 0)] += need_to_beat; // Добавляем очки за блок игроку, который его захватил.
            TutorialMapGenerator.map[a, b].owner = (TutorialMapGenerator.turn ? 1 : 0); // Меняем в матрице "обладателя".
            if(opponent_is_real) // Если блок чужой.
                TutorialMapGenerator.players[(TutorialMapGenerator.turn ? 0 : 1)] -= need_to_beat; // Отнимаем очки у соперника.
            TutorialMapGenerator.turn_count--; // Отнимаем один блок.
        }
    }
}