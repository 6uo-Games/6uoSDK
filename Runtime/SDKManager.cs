using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class SDKManager : MonoBehaviour
{

    AndroidJavaObject unityActivity;
    AndroidJavaObject mSDKHandler;

    string ad_session_token;

    [SerializeField]
    GameObject loginUI;

    [SerializeField]
    GameObject SDKUI;

    [SerializeField]
    InputField mEmailAddress;

    [SerializeField]
    InputField mPassword;

    [SerializeField]
    InputField mConfirmPassword;

    [SerializeField]
    Text mMessageLog;

    [SerializeField]
    string advertisingId;

    [SerializeField]
    string authoizationKey;

    void Awake(){
        mMessageLog.text = "";
        DontDestroyOnLoad( this.gameObject );
        loginUI.SetActive( true );
        SDKUI.SetActive( false );
    }

    void Start() {
        #if UNITY_EDITOR

        #elif UNITY_ANDROID
        AndroidJavaClass unityPlayerClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        unityActivity = unityPlayerClass.GetStatic<AndroidJavaObject>("currentActivity");
        #endif
    }

    // Authentication

    public void Login(){

        object[] parameters = new object[2];
        parameters[0] = mEmailAddress.text;
        parameters[1] = mPassword.text;
        
        #if UNITY_EDITOR

        Debug.Log( parameters[1] );
        loginUI.SetActive( false );
        SDKUI.SetActive( true );
        SceneManager.LoadScene( 1 );

        #elif UNITY_ANDROID

        object[] unityParameters = new object[1];
        unityParameters[0] = unityActivity;
        mSDKHandler = new AndroidJavaObject( "com.erwin.mylibrary.SDKManager", unityParameters );

        mMessageLog.text = mSDKHandler.Call<string>("login", parameters);

        if (mMessageLog.text == "Success"){
            loginUI.SetActive( false );
            SDKUI.SetActive( true );
            SceneManager.LoadScene( 1 );
        }

        #elif UNITY_IPHONE

        string message = IOSPluginInterface.LoginAPI( mEmailAddress.text, mPassword.text );

        if (message == "Success") {
            loginUI.SetActive( false );
            SDKUI.SetActive( true );
            SceneManager.LoadScene( 1 );
        }

        #endif

    }

    public void Signup(){

        object[] parameters = new object[3];
        parameters[0] = mEmailAddress.text;
        parameters[1] = mPassword.text;
        parameters[2] = mConfirmPassword.text;

        #if UNITY_EDITOR

        Debug.Log( parameters[1] );

        #elif UNITY_ANDROID

        object[] unityParameters = new object[1];
        unityParameters[0] = unityActivity;
        mSDKHandler = new AndroidJavaObject( "com.erwin.mylibrary.SDKManager", unityParameters );

        mMessageLog.text = mSDKHandler.Call<string>("signup", parameters);

        #elif UNITY_IPHONE

        string message =IOSPluginInterface.SignupAPI( mEmailAddress.text, mPassword.text, mConfirmPassword.text );

        Debug.Log( message );

        #endif

    }

    // Campaign

    public void initialize_ad(){

        object[] parameters = new object[1];
        parameters[0] = advertisingId;
        string result = "";

        #if UNITY_EDITOR

        #elif UNITY_ANDROID

        object[] unityParameters = new object[1];
        unityParameters[0] = unityActivity;
        mSDKHandler = new AndroidJavaObject( "com.erwin.mylibrary.SDKManager", unityParameters );

        result = mSDKHandler.Call<string>("campaign_initialize", parameters);

        #elif UNITY_IPHONE

        string message = IOSPluginInterface.InitializeCampaignAPI( advertisingId );

        Debug.Log( message );

        #endif

        return;

    }

    public void play_ad(){

        object[] parameters = new object[1];
        parameters[0] = advertisingId;
        string result = "";

        #if UNITY_EDITOR

            StartCoroutine( setAd("https://cdn-icons-png.flaticon.com/512/9148/9148499.png") );

        #elif UNITY_ANDROID

        object[] unityParameters = new object[1];
        unityParameters[0] = unityActivity;
        mSDKHandler = new AndroidJavaObject( "com.erwin.mylibrary.SDKManager", unityParameters );

        result = mSDKHandler.Call<string>("campaign_play_ad", parameters);

        if (result == "Success"){

            ad_session_token = mSDKHandler.Call<string>("getAd_session_token");

            StartCoroutine( setAd("https://api-demo.6uogames.com:8000/campaign/" + ad_session_token + "/get_ad_resources") );

        }

        #elif UNITY_IPHONE

        ad_session_token = IOSPluginInterface.PlayCampaignAPI( advertisingId );

        StartCoroutine( setAd("https://api-demo.6uogames.com:8000/campaign/" + ad_session_token + "/get_ad_resources") );

        #endif

        return;

    }

    public void finish_ad(){

        object[] parameters = new object[2];
        parameters[0] = advertisingId;
        parameters[1] = ad_session_token;
        string result = "";

        #if UNITY_EDITOR
        Debug.Log("Finish Ad is called");
        #elif UNITY_ANDROID

        object[] unityParameters = new object[1];
        unityParameters[0] = unityActivity;
        mSDKHandler = new AndroidJavaObject( "com.erwin.mylibrary.SDKManager", unityParameters );

        result = mSDKHandler.Call<string>("campaign_finish_ad", parameters);

        #elif UNITY_IPHONE

        string message = IOSPluginInterface.FinishCampaignAPI( advertisingId, ad_session_token );

        Debug.Log( message );

        #endif

        return;

    }

    IEnumerator setAd(string url){

        using(var www = UnityWebRequestTexture.GetTexture(url))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                if (www.isDone)
                {
                    var texture = DownloadHandlerTexture.GetContent(www);
                    GameObject panel = new GameObject("Ad Panel");
                    GameObject temp = GameObject.Find("SDKCanvas");
                    panel.transform.SetParent( temp.transform );
                    Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);
                    Image i = panel.AddComponent<Image>();
                    i.sprite = sprite;
                    RectTransform rt = panel.GetComponent<RectTransform>();
                    rt.anchoredPosition = new Vector2( 0, 0 );
                    rt.sizeDelta = new Vector2( Screen.width, Screen.height );

                    yield return new WaitForSeconds(5);

                    GameObject cross = new GameObject("Close Ad");
                    cross.transform.SetParent( temp.transform );
                    Image ii = cross.AddComponent<Image>();
                    Button btn = cross.AddComponent<Button>();
                    ii.sprite = Resources.Load<Sprite>("cross");
                    RectTransform crossRt = cross.GetComponent<RectTransform>();
                    crossRt.anchoredPosition = new Vector2( Screen.width / 2 - 200, Screen.height / 2 - 200 );
                    btn.onClick.AddListener ( delegate(){
                        Debug.Log("Button is pressed!");
                        finish_ad();
                        Destroy( panel );
                        Destroy( cross );
                    } );
                }
            }
        }

    }

    public void Purchcase_pending(){

        #if UNITY_EDITOR

        #elif UNITY_ANDROID

        #elif UNITY_IPHONE

        string message = IOSPluginInterface.PurchasePending( "com.david.200g", authoizationKey );

        Debug.Log( message );

        #endif

        return;

    }

    public void Purchcase_confirm(){

        #if UNITY_EDITOR

        #elif UNITY_ANDROID

        #elif UNITY_IPHONE

        string message = IOSPluginInterface.PurchaseConfirm( authoizationKey );

        Debug.Log( message );

        #endif

        return;

    }
    
}
