using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int score;
    public int ammo;

    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI ammoText;

    void DoNotDestroyOnLoad()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        ScoreRegistry();
    }

    void ScoreRegistry()
    {
        scoreText.text = score.ToString();
        ammoText.text = ammo.ToString();
    }
}
