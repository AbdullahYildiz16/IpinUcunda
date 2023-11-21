using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RestartButton : MonoBehaviour
{
    [SerializeField] Button btn;

    private void Awake()
    {
        btn = gameObject.GetComponent<Button>();
        btn.onClick.AddListener(RestartButtonClicked);
    }
    void RestartButtonClicked()
    {
        SceneManager.LoadScene(0);
    }
}
