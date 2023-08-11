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

    #if UNITY_IPHONE
    [DllImport("__Internal")]
    #endif
    private static extern IntPtr Get6uoBalance();

    public static string check_balance()
    {
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            IntPtr ReceivedMessage = Get6uoBalance();

            return Marshal.PtrToStringAnsi(ReceivedMessage);
        }

        return null;
    }

    #if UNITY_IPHONE
    [DllImport("__Internal")]
    #endif
    private static extern IntPtr SetProd();

    public static string Set_Prod()
    {
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            IntPtr ReceivedMessage = SetProd();

            return Marshal.PtrToStringAnsi(ReceivedMessage);
        }

        return null;
    }

    #if UNITY_IPHONE
    [DllImport("__Internal")]
    #endif
    private static extern IntPtr SetDemo();

    public static string Set_Demo()
    {
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            IntPtr ReceivedMessage = SetDemo();

            return Marshal.PtrToStringAnsi(ReceivedMessage);
        }

        return null;
    }

    #if UNITY_IPHONE
    [DllImport("__Internal")]
    #endif
    private static extern IntPtr Purchase_pending(string product_id, string rsa_public_key);

    public static string PurchasePending(string product_id, string rsa_public_key)
    {
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            IntPtr ReceivedMessage = Purchase_pending(product_id, rsa_public_key);

            return Marshal.PtrToStringAnsi(ReceivedMessage);
        }

        return null;
    }

    #if UNITY_IPHONE
    [DllImport("__Internal")]
    #endif
    private static extern IntPtr Purchase_confirm(string rsa_public_key);

    public static string PurchaseConfirm(string rsa_public_key)
    {
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            IntPtr ReceivedMessage = Purchase_confirm(rsa_public_key);

            return Marshal.PtrToStringAnsi(ReceivedMessage);
        }

        return null;
    }

}