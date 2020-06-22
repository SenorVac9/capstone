using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioMenu : MonoBehaviour
{

    GameManager GameManagerIstance;

    // Start is called before the first frame update
    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

    }

    // Update is called once per frame

    //void Update()
    //{
    //    if (SceneManager.GetActiveScene().name == ("GameScene"))
    //    {
    //        if (GameManagerIstance.GameStart == true)
    //        {
    //            gameObject.SetActive(false);
    //        }
    //    }
    //}
}

