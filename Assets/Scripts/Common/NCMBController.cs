using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using NCMB;

using static CommonUtils;

public static class NCMBController
{
    private static UnityAction _onLogInComplete = null;
    private static UnityAction _onLogInFailed = null;
    private static UnityAction _onLogOutComplete = null;
    private static UnityAction _onLogOutFailed = null;
    private static UnityAction _onSignUpComplete = null;
    private static UnityAction _onSignUpFailed = null;

    private static NCMBUser user;
    
    public static void SignIn(string userName, string password)
    {
        Debug.Log(SHA256(userName, password));
        NCMBUser.LogInAsync(userName, SHA256(userName, password), (error =>
        {
            if (error != null)
            {
                Debug.Log(error.ErrorCode + ":"+ error.ErrorMessage);
                _onLogInFailed?.Invoke();
            }
            else
            {
                _onLogInComplete?.Invoke();
                user = NCMBUser.CurrentUser;
            }
        }));
    }

    public static void SignUp(string userName, string password, string confirmPassword)
    {
        if (!password.Equals(confirmPassword))
        {
            _onSignUpFailed?.Invoke();
            return;
        }

        if (string.IsNullOrEmpty(password))
        {
            _onSignUpFailed?.Invoke();
            return;
        }
        
        user = new NCMBUser();
        user.UserName = userName;
        user.Password = SHA256(userName, password);
        
        user.SignUpAsync((error =>
        {
            if (error != null)
            {
                Debug.Log(error.ErrorCode + ":"+ error.ErrorMessage);
                _onSignUpFailed?.Invoke();
            }
            else
            {
                _onSignUpComplete?.Invoke();
                user = NCMBUser.CurrentUser;
            }
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
            {
                _onLogOutComplete?.Invoke();
                user = null;
            }
        } ));
    }

    public static string GetObjectID()
    {
        return user?.ObjectId;
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
        _onSignUpFailed = callback;
    }
}
