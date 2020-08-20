using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GroupMenuManager : MonoBehaviour
{
    public static GroupMenuManager Instance;

    [SerializeField] private Button _configButton;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        _configButton.onClick.AddListener(() =>
        {
            Debug.Log("こんふぐ");
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
