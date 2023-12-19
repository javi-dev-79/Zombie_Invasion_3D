using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class PlayerLogic : MonoBehaviour
{

    public Life life;
    public bool life0 = false;
    [SerializeField] private Animator loseAnimator;


    // Start is called before the first frame update
    void Start()
    {
        life = GetComponent<Life>();
    }

    // Update is called once per frame
    void Update()
    {
        LifeReview();
    }

    void LifeReview()
    {
        if (life0) return;
        if (life.value <= 0)
        {
            life0 = true;
            Invoke("RestartGame", 2f);
        }
    }

    void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
