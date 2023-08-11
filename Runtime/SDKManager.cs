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
    GameObject PurchaseUI;

    [SerializeField]
    InputField mEmailAddress;

    [SerializeField]
    InputField mPassword;

    [SerializeField]
    InputField mConfirmPassword;

    [SerializeField]
    Text mMessageLog;

    [SerializeField]
    Text mBalance;

    [SerializeField]
    string advertisingId;

    [SerializeField]
    string authoizationKey;

    [SerializeField]
    bool ProdMode;

    void Awake(){
        mMessageLog.text = "";
        DontDestroyOnLoad( this.gameObject );
        loginUI.SetActive( true );
        SDKUI.SetActive( false );
        PurchaseUI.SetActive( false );
    }

    void Start() {

        #if UNITY_EDITOR

        #elif UNITY_ANDROID

        AndroidJavaClass unityPlayerClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        unityActivity = unityPlayerClass.GetStatic<AndroidJavaObject>("currentActivity");

        if (ProdMode){
            object[] unityParameters = new object[1];
            unityParameters[0] = unityActivity;
            mSDKHandler = new AndroidJavaObject( "com.erwin.mylibrary.SDKManager", unityParameters );

            mSDKHandler.Call("SetProd");
        }

        #elif UNITY_IPHONE

        if (ProdMode)
            IOSPluginInterface.Set_Prod();
        else
            IOSPluginInterface.Set_Demo();

        #endif
    }

    // Authentication

    public void Login(){

        object[] parameters = new object[2];
        parameters[0] = mEmailAddress.text;
        parameters[1] = mPassword.text;
        
        #if UNITY_EDITOR

        loginUI.SetActive( false );
        SDKUI.SetActive( true );
        PurchaseUI.SetActive( true );
        SceneManager.LoadScene( 1 );

        #elif UNITY_ANDROID

        object[] unityParameters = new object[1];
        unityParameters[0] = unityActivity;
        mSDKHandler = new AndroidJavaObject( "com.erwin.mylibrary.SDKManager", unityParameters );

        mMessageLog.text = mSDKHandler.Call<string>("login", parameters);

        #elif UNITY_IPHONE

        mMessageLog.text = IOSPluginInterface.LoginAPI( mEmailAddress.text, mPassword.text );

        #endif

        if (mMessageLog.text == "Success"){
            loginUI.SetActive( false );
            SDKUI.SetActive( true );
            PurchaseUI.SetActive( true );
            InvokeRepeating("check_balance", 0.0f, 5.0f);
            SceneManager.LoadScene( 1 );
        }

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

        mMessageLog.text = IOSPluginInterface.SignupAPI( mEmailAddress.text, mPassword.text, mConfirmPassword.text );


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
                    PurchaseUI.SetActive( false );
                    var texture = DownloadHandlerTexture.GetContent(www);
                    GameObject panel = new GameObject("Ad Panel");
                    GameObject temp = GameObject.Find("AdCanvas");
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
                        PurchaseUI.SetActive( true );
                        Destroy( panel );
                        Destroy( cross );
                    } );
                }
            }
        }

    }

    public void Purchcase_pending(string item){

        #if UNITY_EDITOR

        #elif UNITY_ANDROID

        object[] parameters = new object[2];
        parameters[0] = item;
        parameters[1] = authoizationKey;
        string result = "";
        object[] unityParameters = new object[1];
        unityParameters[0] = unityActivity;
        mSDKHandler = new AndroidJavaObject( "com.erwin.mylibrary.SDKManager", unityParameters );

        result = mSDKHandler.Call<string>("purchase_pending", parameters);

        #elif UNITY_IPHONE

        string message = IOSPluginInterface.PurchasePending( item, authoizationKey );

        Debug.Log( message );

        #endif

        return;

    }

    public void Purchcase_confirm(){

        #if UNITY_EDITOR

        #elif UNITY_ANDROID

        object[] parameters = new object[1];
        parameters[0] = authoizationKey;
        string result = "";
        object[] unityParameters = new object[1];
        unityParameters[0] = unityActivity;
        mSDKHandler = new AndroidJavaObject( "com.erwin.mylibrary.SDKManager", unityParameters );

        result = mSDKHandler.Call<string>("purchase_confirm", parameters);

        #elif UNITY_IPHONE

        string message = IOSPluginInterface.PurchaseConfirm( authoizationKey );

        Debug.Log( message );

        #endif

        return;

    }

    public string check_balance(){

        string balance = "";

        #if UNITY_EDITOR

        #elif UNITY_ANDROID

        object[] unityParameters = new object[1];
        unityParameters[0] = unityActivity;
        mSDKHandler = new AndroidJavaObject( "com.erwin.mylibrary.SDKManager", unityParameters );

        string result = mSDKHandler.Call<string>("updateBalance");
        balance = mSDKHandler.Call<string>("getBalance");

        #elif UNITY_IPHONE

        balance = IOSPluginInterface.check_balance(  );

        #endif

        if (balance != ""){
            float balanceFloat = float.Parse( balance ) / 1000000000.0f;
            mBalance.text = balanceFloat.ToString();
        }

        return balance;

    }
    
}
