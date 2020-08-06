using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NCMB;

public class TextChat : MonoBehaviour
{
    public static TextChat instance;

    private void Awake() { instance = this; }

    // Start is called before the first frame update
    void Start()
    {
        // AddTestFriend();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddTestFriend()
    {
        NCMBObject obj = new NCMBObject("Friend");
        
        ArrayList array = new ArrayList ();
        // NCMBUser.GetQuery();
        array.Add ("123");
        array.Add ("456");
        obj.AddRangeUniqueToList ("FriendNameList", array);
        obj.SaveAsync ();
    }
}
