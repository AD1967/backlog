using System.Collections;
using UnityEngine.Animations;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UiEvents : MonoBehaviour
{
    //Звук клика который используется почти везде
    public AudioClip Click;
    //Контроль чтобы все иконки звука были актуальными
    public Text SoundText;
    public Text BotText;
    public AnimationClip Animat;
    public Animator ButtonAnimator;
    public Dropdown MapSizeControl, MapTypeControl;
    

    /// <summary>
    /// Фукция которая загружает сцену при нажатие кнопки
    /// </summary>
    public void LoadScene(int Number)
    {
        //Воспроизведение звука
        PlaySound(Click);
        //Загрузка сцены
        if(Number == 1 && PlayerScore.new_player == 0){
            Number = 3;
        }
        SceneManager.LoadSceneAsync(Number);
    }
    /// <summary>
    /// Фукция которая перезагружает сцену при нажатие кнопки
    /// </summary>
    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    /// <summary>
    /// Функция которая открывает ссылку при нажатие кнопки
    /// </summary>
    public void OpenURL(string URL)
    {
        //Воспроизведение звука
        PlaySound(Click);
        //Открытие ссылки
        Application.OpenURL(URL);
    }
    

    private void Update()
    {

        SoundControl();
        BotControl();

        // Debug.Log(MapGenerator.type);
    }
   

    /// <summary>
    /// Контролирует, чтобы кнопка которая отвечает за звук
    /// если он отключен показывала иконку выключеного звука
    /// </summary>
    private void BotControl()
    {
        //Если не используется то скрипт не выдаёт ошибки
        if (BotText == null)
            return;

        //Если звук ниже нуля то меняем иконку
        if (GameSettings.GameWithBot)
        {
            //Изменяем иконку
            BotText.text = "";
            //Остановка
            return;
        }
        //Иконка что звук идёт
        BotText.text = "";
    }

    public void SetGameWithBot(Toggle toggle)
    {
        PlaySound(Click);
        StartCoroutine(Anim(toggle.isOn));
    }
  
    private IEnumerator Anim(bool active)
    {
        ButtonAnimator.SetBool("Hide", true);
        yield return new WaitForSeconds(0.5f);
        GameSettings.GameWithBot = active;
        ButtonAnimator.SetBool("Hide", false);
    }

    private void Start()
    {
        DontDestroyOnLoad(this.gameObject);

        if (MapSizeControl != null)
            switch (MapGenerator.n)
            {
                case 3:
                    MapSizeControl.value = 0;
                    break;
                case 4:
                    MapSizeControl.value = 1;
                    break;
                case 5:
                    MapSizeControl.value = 2;
                    break;
                case 6:
                    MapSizeControl.value = 3;
                    break;
            }
        if (MapTypeControl != null)
            switch (MapGenerator.type)
            {
                case 4:
                    MapTypeControl.value = 0;
                    break;
                case 6:
                    MapTypeControl.value = 1;
                    break;
                case 8:
                    MapTypeControl.value = 2;
                    break;
            }
    }

    /// <summary>
    /// Контролирует, чтобы кнопка которая отвечает за звук
    /// если он отключен показывала иконку выключеного звука
    /// </summary>
    private void SoundControl()
    {
        //Если не используется то скрипт не выдаёт ошибки
        if (SoundText == null)
            return;
        
        //Если звук ниже нуля то меняем иконку
        if (GameSettings.GameVolume == 0f)
        {
            //Изменяем иконку
            SoundText.text = "";
            //Остановка
            return;
        }
        //Иконка что звук идёт
        SoundText.text = "";
    }
    /// <summary>
    /// При нажатие на кнопку функция выключает звук вообще
    /// </summary>
    public void MuteSound(Text text)
    {
        //Воспроизведение звука
        PlaySound(Click);

        //Выбор иконки
        switch (text.text)
        {
            //Если иконка включенного звука то он переключает на противоположную
            case ""://Не звук
                text.text = "";//Меняем на звук
                GameSettings.GameVolume = 100f; //Ставим звук на максимум
                break;

            //Если иконка выключеного звука то он переключает на противоположную
            case ""://Звук
                text.text = "";//Меняем на не звук
                GameSettings.GameVolume = 0f; //Выключаем звук

                break;
        }
    }

    /// <summary>
    /// Функция которая воспроизводит звук
    /// </summary>
    private void PlaySound(AudioClip sound)
    {
      
        //Объявляем компонент на объекте скрипта
        var Source = GetComponent<AudioSource>();
        //Меняем звук в зависимости на которые в настройках игры
        Source.volume = GameSettings.GameVolume;
        //Проигрываем звук
        Source.PlayOneShot(sound);
    }

    public void SetSizeMap(int Size)
    {
        switch (Size)
        {
            case 0:
                MapGenerator.n = 3;
                break;
            case 1:
                MapGenerator.n = 4;
                break;
            case 2:
                MapGenerator.n = 5;
                break;
            case 3:
                MapGenerator.n = 6;
                break;
        }
    }

    public void SetTypeMap(int Type){
        switch(Type){
            case 0: 
                MapGenerator.type = 4;
                break;
            case 1: 
                MapGenerator.type = 6;
                break;
            case 2: 
                MapGenerator.type = 8;
                break;    
        }
    }

}
//  There were paws Felix Only (felixdeveloperkettle@gmail.com)
//  Author :    Felix Only (felixdeveloperkettle@gmail.com) 
//  Date     :    29 May 2020 