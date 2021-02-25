using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// Скрипт для автоподстройки положения камеры.
// Тут будет почти чистая математика.
// Алгоритм реализован только для квадратных карт,
// потому что с прямоугольными слишком тяжкие чета расчеты у меня получились

public class AutoPositionCameraScript : MonoBehaviour
{
    public static double angle_to_map = 45; // Угол под которым камера стоит относительно карты.
    double radians_of_angle = angle_to_map * (Math.PI / 180);
    double delta = 0.99; // Часть заполнения карты слева направо

    
    void Start(){
        int n = MapGenerator.n, m = MapGenerator.m;
        float block_lenght = MapGenerator.block_lenght, r = MapGenerator.r;
        // Переносим значения со скрипта генерации сюда. Чисто для удобства чтения кода.
        
        var camera = gameObject.GetComponent<Camera>();
        double field_of_view = camera.fieldOfView * delta * ((float)camera.pixelWidth / (float)camera.pixelHeight); // Угол видимости камеры.
        double field_of_view_radians = field_of_view * (Math.PI / 180);

        // Определяем прямую на которой лежит камера
        var first_block_up = new Vector3(0, block_lenght / 2, 0); // Это центр поверхности первого блока 
        var last_block_up = new Vector3((n - 1) * (block_lenght + r), block_lenght / 2, (m - 1) * (block_lenght + r)); // последнего блока
        var centre_up = (first_block_up + last_block_up) / 2; // Это получился центр поверхности карты.
        // Прямая, которая содержит first_block_up и centre_up -- это проекция прямой, на которой будет лежать камера на плоскость поверхности карты

        var left_edge = new Vector3(-block_lenght / 2, block_lenght / 2, block_lenght / 2 + (m - 1) * (block_lenght + r)); // Левая крайняя точка карты
        var right_edge = new Vector3(block_lenght / 2 + (n - 1) * (block_lenght + r), block_lenght / 2, - block_lenght / 2); // Правая крайняя точка карты
        var left_to_right_dist = Vector3.Distance(left_edge, right_edge);

        var side_radians = (180 - field_of_view) / 2 * (Math.PI / 180); // боковой угол в радианах 
        var side_dist = left_to_right_dist / Math.Sqrt((2 - 2 * Math.Cos(field_of_view_radians)));
        double camera_distance = Math.Sqrt(side_dist * side_dist - (left_to_right_dist / 2) * (left_to_right_dist / 2));
        camera_distance = camera_distance * Math.Cos(radians_of_angle);

        var height = centre_up.y + Math.Tan(radians_of_angle) * (camera_distance); // Координата камеры по Oy
        var zx = centre_up.x - camera_distance / Math.Sqrt(2); // Координата по Ох, по совместительству и по Oz

        gameObject.transform.position = new Vector3((float)zx, (float)height, (float)zx); // Ставим камеру на нужное место
        
        var rotationVector = transform.rotation.eulerAngles; // Угол под которым будет смотреть камера
        rotationVector.x = (float)angle_to_map; // Под каким углом камера находится от центра, под таким и смотрит вниз.
        rotationVector.y = (float)(Math.Atan((double)n / (double)m) * 180 / Math.PI); // А это просто 45 градусов, ну потому что нам нужно пройти одну восьмую круга чтобы камера смотрела ровно на центр карты
        gameObject.transform.rotation = Quaternion.Euler(rotationVector); // Поворачиваем камеру

    }

    void Update(){
        // В будущем будет написан алгоритм поворота карты по свайпу (Это красиво, удобно, интуитивно).
    }
}
