using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GroupMenuManager : MonoBehaviour
{
    [SerializeField] private Button _configButton;

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
