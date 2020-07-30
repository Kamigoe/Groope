using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using NCMB;

public static class NCMBController
{
    private static UnityAction _onLogInComplete = null;
    private static UnityAction _onLogInFailed = null;
    private static UnityAction _onLogOutComplete = null;
    private static UnityAction _onLogOutFailed = null;
    private static UnityAction _onSignUpComplete = null;
    private static UnityAction _onSignUpInFailed = null;
    
    public static void SignIn(string userName, string password)
    {
        NCMBUser.LogInAsync(userName, password, (error =>
        {
            if (error != null)
            {
                Debug.Log(error.ErrorCode + ":"+ error.ErrorMessage);
                _onLogInFailed?.Invoke();
            }
            else
                _onLogInComplete?.Invoke();
        }));
    }

    public static void SignUp(string userName, string password, string confirmPassword)
    {
        if (!password.Equals(confirmPassword)) return;
        
        NCMBUser user = new NCMBUser();
        user.UserName = userName;
        user.Password = password;
        
        user.SignUpAsync((error =>
        {
            if (error != null)
            {
                Debug.Log(error.ErrorCode + ":"+ error.ErrorMessage);
                _onSignUpInFailed?.Invoke();
            }
            else
                _onSignUpComplete?.Invoke();
        } ));
    }

    public static void SignOut()
    {
        NCMBUser.LogOutAsync((error =>
        {
            if (error != null)
            {
                Debug.Log(error.ErrorCode + ":"+ error.ErrorMessage);
                _onLogOutFailed?.Invoke();
            }
            else
                _onLogOutComplete?.Invoke();
        } ));
    }

    public static void OnSignInComplete(UnityAction callback)
    {
        _onLogInComplete = callback;
    }
    public static void OnSignInFailed(UnityAction callback)
    {
        _onLogInFailed = callback;
    }
    public static void OnSignOutComplete(UnityAction callback)
    {
        _onLogOutComplete = callback;
    }
    public static void OnSignOutFailed(UnityAction callback)
    {
        _onLogOutFailed = callback;
    }
    public static void OnSignUpComplete(UnityAction callback)
    {
        _onSignUpComplete = callback;
    }
    public static void OnSignUpFailed(UnityAction callback)
    {
        _onSignUpInFailed = callback;
    }
}
