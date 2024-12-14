using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HudManager : MonoBehaviour
{
    [SerializeField]
    List<TextMeshProUGUI> scoreTextList;

    List<int> score;

    private void Awake()
    {
        score = new List<int>(new int[scoreTextList.Count]);
    }

    public void AddScoreText(int index)
    {
        ++score[index];
        scoreTextList[index].text = $"Score {score[index]}";
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
