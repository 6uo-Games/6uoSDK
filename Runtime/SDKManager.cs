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
    InputField mID;

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

    GameObject canvas;
    AsyncOperation scene;
    GameObject startBtn;
    GameObject shopBtn;

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

        scene = SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);
        scene.allowSceneActivation = false;

    }

    // Authentication

    public void Login(){

        object[] parameters = new object[2];
        parameters[0] = mEmailAddress.text;
        parameters[1] = mPassword.text;
        
        #if UNITY_EDITOR

        loginUI.SetActive( false );
        SDKUI.SetActive( true );
        scene.allowSceneActivation = true;

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
            check_balance();
            SceneManager.LoadScene( 1 );
        }

    }

    public void Signup(){

        object[] parameters = new object[4];
        parameters[0] = mEmailAddress.text;
        parameters[1] = mID.text;
        parameters[2] = mPassword.text;
        parameters[3] = mConfirmPassword.text;

        #if UNITY_EDITOR

        Debug.Log( parameters[1] );

        #elif UNITY_ANDROID

        object[] unityParameters = new object[1];
        unityParameters[0] = unityActivity;
        mSDKHandler = new AndroidJavaObject( "com.erwin.mylibrary.SDKManager", unityParameters );

        mMessageLog.text = mSDKHandler.Call<string>("signup", parameters);

        #elif UNITY_IPHONE

        mMessageLog.text = IOSPluginInterface.SignupAPI( mEmailAddress.text, mID.text, mPassword.text, mConfirmPassword.text );


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

            if (!ProdMode)
                StartCoroutine( setAd("https://api-demo.6uogames.com:8000/campaign/" + ad_session_token + "/get_ad_resources") );
            else
                StartCoroutine( setAd("https://api.6uogames.com:8000/campaign/" + ad_session_token + "/get_ad_resources") );

        }else if (result == "DEMO OK"){
            StartCoroutine( setAd("https://api.6uogames.com:8000/campaign/preview/get_ad_resources") );
        }

        #elif UNITY_IPHONE

        ad_session_token = IOSPluginInterface.PlayCampaignAPI( advertisingId );

        if (ad_session_token != null){

            if (!ProdMode)
                StartCoroutine( setAd("https://api-demo.6uogames.com:8000/campaign/" + ad_session_token + "/get_ad_resources") );
            else
                StartCoroutine( setAd("https://api.6uogames.com:8000/campaign/" + ad_session_token + "/get_ad_resources") );
                
        }else {
            StartCoroutine( setDemoAd("https://api.6uogames.com:8000/campaign/preview/get_ad_resources") );
        }

        #endif

        canvas = GameObject.Find("Canvas");
        canvas.SetActive(false);
        Time.timeScale = 0.0f;

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

        canvas.SetActive(true);
        check_balance();

        return;

    }

    IEnumerator setDemoAd(string url){

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
                    Time.timeScale = 1.0f;
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
                        Destroy( panel );
                        Destroy( cross );
                    } );
                }
            }
        }

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
                    Time.timeScale = 1.0f;
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

    public void Purchcase_pending(string item){

        string result = "";

        #if UNITY_EDITOR

        #elif UNITY_ANDROID

        object[] parameters = new object[2];
        parameters[0] = item;
        parameters[1] = authoizationKey;
        object[] unityParameters = new object[1];
        unityParameters[0] = unityActivity;
        mSDKHandler = new AndroidJavaObject( "com.erwin.mylibrary.SDKManager", unityParameters );

        result = mSDKHandler.Call<string>("purchase_pending", parameters);

        #elif UNITY_IPHONE

        result = IOSPluginInterface.PurchasePending( item, authoizationKey );

        #endif

        if ( result == "DEMO OK" ){
            // DEMO

        }else if ( result == "Success" ){
            // Success

        }

        return;

    }

    public void Purchcase_confirm(){

        string result = "";

        #if UNITY_EDITOR

        #elif UNITY_ANDROID

        object[] parameters = new object[1];
        parameters[0] = authoizationKey;
        object[] unityParameters = new object[1];
        unityParameters[0] = unityActivity;
        mSDKHandler = new AndroidJavaObject( "com.erwin.mylibrary.SDKManager", unityParameters );

        result = mSDKHandler.Call<string>("purchase_confirm", parameters);

        #elif UNITY_IPHONE

        result = IOSPluginInterface.PurchaseConfirm( authoizationKey );

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

    public void HideStartBtn(){
        startBtn = GameObject.Find("StartButton");
        startBtn.SetActive(false);
    }

    public void ShowStartBtn(){
        startBtn.SetActive(true);
    }

    public void HideShopBtn(){
        shopBtn = GameObject.Find("Shop");
        shopBtn.SetActive(false);
    }
    
}
