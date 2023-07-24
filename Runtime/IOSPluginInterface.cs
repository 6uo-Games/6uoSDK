using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using System;

public class IOSPluginInterface : MonoBehaviour
{

    #if UNITY_IPHONE
    [DllImport("__Internal")]   
    #endif
    private static extern IntPtr SignUpWithEmail(string emailAddress, string password, string confirmPassword);

    public static string SignupAPI(string emailAddress, string password, string confirmPassword)
    {         
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            IntPtr ReceivedMessage = SignUpWithEmail(emailAddress, password, confirmPassword);

            return Marshal.PtrToStringAnsi(ReceivedMessage);
        }
        
        return null;
    }

    #if UNITY_IPHONE
    [DllImport("__Internal")]
    #endif
    private static extern IntPtr LoginWithEmail(string emailAddress, string password);

    public static string LoginAPI(string email, string password)
    {
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            IntPtr ReceivedMessage = LoginWithEmail(email, password);

            return Marshal.PtrToStringAnsi(ReceivedMessage);
        }

        return null;
    }

    #if UNITY_IPHONE
    [DllImport("__Internal")]
    #endif
    private static extern IntPtr InitializeAd(string advertising_id);

    public static string InitializeCampaignAPI(string advertisingId)
    {
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            IntPtr ReceivedMessage = InitializeAd(advertisingId);

            return Marshal.PtrToStringAnsi(ReceivedMessage);
        }

        return null;
    }

    #if UNITY_IPHONE
    [DllImport("__Internal")]
    #endif
    private static extern IntPtr PlayAd(string advertising_id);

    public static string PlayCampaignAPI(string advertisingId)
    {
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            IntPtr ReceivedMessage = PlayAd(advertisingId);

            return Marshal.PtrToStringAnsi(ReceivedMessage);
        }

        return null;
    }

    #if UNITY_IPHONE
    [DllImport("__Internal")]
    #endif
    private static extern IntPtr FinishAd(string advertising_id, string ad_session_token);

    public static string FinishCampaignAPI(string advertisingId, string ad_session_token)
    {
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            IntPtr ReceivedMessage = FinishAd(advertisingId, ad_session_token);

            return Marshal.PtrToStringAnsi(ReceivedMessage);
        }

        return null;
    }
}