using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using UnityEngine.Networking;
using UnityEngine.UI;

public class VideoControllerScript : MonoBehaviour
{

    public bool IsImageComparator;
    public UnityEngine.Video.VideoPlayer[] VideoPlayers;

    private bool isReady;

    string path;
    string jsonString;
    private int numberOfVideosThatArePrepared=1;
    VideoFiles videoFiles;
    public int numberOfVideos=0;
    public bool isPlaying;
    public float duration;
    public Slider progressSlider;
    public GameObject[] VideoSliders;
    public string[] Tags;
    //public RectTransform canvasRT;
    public float aspectRatio;
    public Text debug;
    //public Text framesText;

    public bool flipSliders;
    public float interSliderDistance;

    public RawImage[] videoRawImages;
    public int maxRes;
    //public RectTransform test;
    //VideosContainer VideosCont;
    void Start()
    {
        if(IsImageComparator)
        {
            videoRawImages[0].enabled = true;
            videoRawImages[1].enabled = true;

            SliderScript.sliderValues = new float[1] { 0.5f };
            VideoSliders[1].SetActive(true);

            Texture2D tempTexture= Resources.Load<Texture2D>("Image1");
            videoRawImages[0].texture = tempTexture;
            videoRawImages[1].texture = Resources.Load<Texture2D>("Image2");

            aspectRatio = 4f / 3f;
            Debug.Log(aspectRatio);
        }
        else
        {
#if UNITY_WEBGL

            GetStartupFile();

#endif

        }



    }

    float tempLoc = 0;
    float tempTime = 0;

    private void SetTime(float timeTemp)
    {

        tempTime = timeTemp;

        for (int i = 0; i < numberOfVideos; i++)
        {
            VideoPlayers[i].externalReferenceTime = tempTime;
            tempLoc = tempLoc + (float)VideoPlayers[i].clockTime;
        }
        tempLoc = tempLoc / numberOfVideos;

        progressSlider.value = tempLoc / duration;

        tempLoc = 0;

    }


    private void Update()
    {
        // Check progress

        if(isReady)
        {
            SetTime(Time.time);

            //framesText.text = "Frame: " + VideoPlayers[0].frame + " | " + VideoPlayers[1].frame + " | " + VideoPlayers[2].frame;
            //framesText.text = framesText.text+ "\nTime (s): " + VideoPlayers[0].time.ToString("00.000") + " | " + VideoPlayers[1].time.ToString("00.000") + " | " + VideoPlayers[2].time.ToString("00.000");
            //VideoPlayers[1].frame = VideoPlayers[0].frame;
            //VideoPlayers[2].frame = VideoPlayers[0].frame;
            if (debug.IsActive())
            {
                //Debug.Log("Active");
                //debug.text="Can skip on drop: "+VideoPlayers[0].skipOnDrop;
                debug.text = "Video Player Clock Time in s: " + VideoPlayers[0].clockTime;
                debug.text = debug.text + "\nClip Time (HH:MM:SS): " + (VideoPlayers[0].time / (60 * 60)).ToString("00") + ":" + (VideoPlayers[0].time / (60)).ToString("00") + ":" + (VideoPlayers[0].time).ToString("00");

                debug.text = debug.text + "\nExternal Reference Time: " + VideoPlayers[0].externalReferenceTime;
                //debug.text = debug.text + "\nCan set time source: " + VideoPlayers[0].canSetTimeSource;
                debug.text = debug.text + "\nTime in s: " + VideoPlayers[0].time.ToString("00.000") + " | " + VideoPlayers[1].time.ToString("00.000") + " | " + VideoPlayers[2].time.ToString("00.000");

                debug.text = debug.text + "\nFrame Rates: " + VideoPlayers[0].frameRate + " | " + VideoPlayers[1].frameRate + " | " + VideoPlayers[2].frameRate;
                debug.text = debug.text + "\nCurrent Frame: " + VideoPlayers[0].frame + " | " + VideoPlayers[1].frame + " | " + VideoPlayers[2].frame;
                debug.text = debug.text + "\nFrame Count: " + VideoPlayers[0].frameCount + " | " + VideoPlayers[1].frameCount + " | " + VideoPlayers[2].frameCount;

                debug.text = debug.text + "\nDuration: " + (float)VideoPlayers[0].length + " | " + (float)VideoPlayers[1].length + " | " + (float)VideoPlayers[2].length;


            }

            //Debug.Log("can set skip:  " + VideoPlayers[0].skipOnDrop + "  can?: " + VideoPlayers[0].canSetSkipOnDrop);
        }


    }

    private void OnDestroy()
    {
        for (int i = 0; i < numberOfVideos; i++)
        {
            VideoPlayers[i].prepareCompleted -= DoTheseOncePrepared;
            VideoPlayers[i].loopPointReached -= OnVideoEndReached;
        }

    }

    //// Update is called once per frame
    //void Update()
    //{
    //    Debug.Log(test.sizeDelta);
    //}

    private void DoTheseOncePrepared(UnityEngine.Video.VideoPlayer vp)
    {
        if (numberOfVideosThatArePrepared==numberOfVideos)
        {
            CheckIfReady();

            if(isReady)
            {
                Debug.Log("After prepared");
                Debug.Log("Prepared and playing  " + numberOfVideosThatArePrepared + "  " + numberOfVideos);


                EnableSliders();
                duration = (float)vp.length;
                Play();
                Pause();
                SetAspectRatio();

                if(AreVideosOfSameAspectRatio())
                {
                    for (int i = 0; i < numberOfVideos; i++)
                    {
                        SetRenderTextureSize(VideoPlayers[i].texture.width, VideoPlayers[i].texture.height, VideoPlayers[i]);
                    }


                    if (invokedFromDialogue)
                    {
                        Play();
                        invokedFromDialogue = false;
                    }

                }
                else
                {
                    isReady = false;
                    SetErrorTexture();
                    Debug.Log("Unequal Aspect Ratios");
                }





            }



            numberOfVideosThatArePrepared = 1; // reset for future calls
        }
        else
        {
            Debug.Log("Not playing");
            numberOfVideosThatArePrepared++;
        }
        
    }

    private void SetAspectRatio()
    {
        aspectRatio = (float)VideoPlayers[0].width/(float)VideoPlayers[0].height;

        //Debug.Log("AR:  "+ aspectRatio + "w h"+ VideoPlayers[0].width + " " + VideoPlayers[0].height);
        //canvasRT.sizeDelta = new Vector2(canvasRT.sizeDelta.x*aspectRatio,canvasRT.sizeDelta.y);


    }
    //private bool IsPrepared()
    //{
    //    bool tempBool = true;
    //    for (int i=0; i<numberOfVideos;i++)
    //    {
    //        if(!VideoPlayers[i].isPrepared)
    //        {
    //            tempBool = false;
    //        }
    //    }
    //    return tempBool;
    //}

    public void EnableSliders()
    {
        if(numberOfVideos>1)
        {
            SliderScript.sliderValues = new float[numberOfVideos - 1];

            switch (numberOfVideos)
            {
                case 2:

                    ActivateSlider(1, 0);

                    break;
                case 3:

                    GameObject sliderTemp1 = ActivateSlider(1, 0);
                    GameObject sliderTemp2 = ActivateSlider(2, 2);

                    SetSliderPosition(sliderTemp1, sliderTemp2);

                    break;
                default:
                    break;
            }


        }

    }

    private void SetSliderPosition(GameObject GO1, GameObject GO2)
    {
        if(GO1.GetComponent<SliderScript>().isVertical && GO2.GetComponent<SliderScript>().isVertical)
        {
            //Debug.Log("Both Vertical");

            Vector3 tempPos = GO1.GetComponent<RectTransform>().localPosition;
            GO1.GetComponent<RectTransform>().localPosition = new Vector3(interSliderDistance, tempPos.y, tempPos.z);

            GO1.GetComponent<Slider>().value = 2f / 3f;

            tempPos = GO2.GetComponent<RectTransform>().localPosition;
            GO2.GetComponent<RectTransform>().localPosition = new Vector3(-interSliderDistance, tempPos.y, tempPos.z);

            GO2.GetComponent<Slider>().value = 1f / 3f;

        } else if(!GO1.GetComponent<SliderScript>().isVertical && !GO2.GetComponent<SliderScript>().isVertical)
        {
            //Debug.Log("Both Horizontal");

            Vector3 tempPos = GO1.GetComponent<RectTransform>().localPosition;
            GO1.GetComponent<RectTransform>().localPosition = new Vector3(tempPos.x, -(GO1.GetComponent<SliderScript>().videoRawImageGO.GetComponent<RectTransform>().sizeDelta.y / 2) + (interSliderDistance), tempPos.z);
            GO1.GetComponent<Slider>().value = 2f / 3f;

            tempPos = GO2.GetComponent<RectTransform>().localPosition;
            GO2.GetComponent<RectTransform>().localPosition = new Vector3(tempPos.x, -(GO1.GetComponent<SliderScript>().videoRawImageGO.GetComponent<RectTransform>().sizeDelta.y / 2) - (interSliderDistance), tempPos.z);
            GO2.GetComponent<Slider>().value = 1f / 3f;
        }
    }

    public float[] DisableSliders()
    {
        float[] tempSliderValues = new float[2];

        int k = 0;
        for (int i=0; i<VideoSliders.Length;i++)
        {

            if(VideoSliders[i].activeInHierarchy)
            {
                //Vector3 tempPos = VideoSliders[i].GetComponent<RectTransform>().localPosition;

                tempSliderValues[k]=VideoSliders[i].GetComponent<Slider>().value;

                VideoSliders[i].SetActive(false);

                //if(VideoSliders[i].GetComponent<SliderScript>().isVertical)
                //{
                //    VideoSliders[i].GetComponent<RectTransform>().localPosition = new Vector3(tempPos.x, 0f, 0f);
                //}
                //else
                //{
                //    VideoSliders[i].GetComponent<RectTransform>().localPosition = new Vector3(0f, tempPos.y, 0f);
                //}


                k++;

            }
        }

        return tempSliderValues;

    }

    private void SetRenderTextureSize(int inW, int inH, UnityEngine.Video.VideoPlayer vp)
    {
#if (UNITY_WEBGL)
        float AR = (float)inW / inH;

        if(AR>=1)
        {
            if(inW>maxRes)
            {
                inW = maxRes;
                inH = Mathf.RoundToInt(maxRes/AR);
            }

        }else
        {
            if(inH>1024)
            {
                inW = Mathf.RoundToInt(maxRes * AR);
                inH = maxRes;
            }

        }

#endif

        RenderTexture RT= new RenderTexture(inW, inH, 16, RenderTextureFormat.ARGB32);

        vp.targetTexture = RT;

        videoRawImages[vp.gameObject.transform.GetSiblingIndex()].texture = RT;

        Debug.Log("Texture size:   " + RT.width + "x" + RT.height);



    }

    private GameObject ActivateSlider( int index, int SlInd)
    {
        if (videoFiles.result[index].isHorizontalSlider)
        {
            if(!flipSliders)
            {
                VideoSliders[SlInd].SetActive(true);

                return VideoSliders[SlInd];
            }
            else
            {
                VideoSliders[SlInd+1].SetActive(true);
                return VideoSliders[SlInd+1];
            }

        }
        else
        {
            if(!flipSliders)
            {
                VideoSliders[SlInd + 1].SetActive(true);

                return VideoSliders[SlInd+1];
            }
            else
            {
                VideoSliders[SlInd].SetActive(true);

                return VideoSliders[SlInd];
            }
           
        }

    }

    // swaps the direction of the sliders
    public void SwapSliders()
    {
        if (flipSliders)
        {
            flipSliders = false;

        }
        else
        {
            flipSliders = true;

        }

        float[] tempSliderValues = DisableSliders();
        EnableSliders();
        SetSliderValues(tempSliderValues);
    }

    private void SetSliderValues(float[] inputValues)
    {
        int k = 0;
        for (int i = 0; i < VideoSliders.Length; i++)
        {

            if (VideoSliders[i].activeInHierarchy)
            {
                VideoSliders[i].GetComponent<Slider>().value = inputValues[k];
                k++;
            }
        }

    }


    private void GetStartupFile()
    {

        path = Path.Combine(Application.streamingAssetsPath, "Startup.json");

#if UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX
        path = "file://" + path;
#endif
        //Debug.Log(path);
        StartCoroutine( GetRequestAndContinue(path));

    }



    private void SetVideoManager()
    {

        Tags = new string[numberOfVideos];
        for(int i=0; i<numberOfVideos;i++)
        {

//#if UNITY_WEBGL || UNITY_STANDALONE

            VideoPlayers[i].url = System.IO.Path.Combine(Application.streamingAssetsPath, videoFiles.result[i].name);
            Tags[i] = videoFiles.result[i].tag;
            VideoPlayers[i].gameObject.SetActive(true);
            VideoPlayers[i].skipOnDrop = true;

            VideoPlayers[i].loopPointReached -= OnVideoEndReached;
            VideoPlayers[i].loopPointReached += OnVideoEndReached;



            VideoPlayers[i].timeReference = UnityEngine.Video.VideoTimeReference.ExternalTime;

//#endif

        }

    }


    IEnumerator GetRequestAndContinue(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            if (webRequest.isNetworkError)
            {
                Debug.Log( "Error: " + webRequest.error);
            }
            else
            {
                jsonString = webRequest.downloadHandler.text;
               videoFiles = JsonUtility.FromJson<VideoFiles>("{\"result\":" + jsonString + "}");

                SetUpAndContinue();
            }
        }
    }

    bool invokedFromDialogue = false;
    public void ResetPlayer(VideoFiles vfIn)
    {
        invokedFromDialogue = true;
        Stop();
        if(videoFiles!=null)
        {
            videoFiles = null;
        }

        videoFiles = vfIn;

        for(int i=0;i<3;i++)
        {
            VideoPlayers[i].gameObject.SetActive(false);
            //VideoPlayers[i].url = "";
        }

        DisableSliders();

        SetUpAndContinue();

    }

    public void SetUpAndContinue()
    {
        numberOfVideos = videoFiles.result.Length;

        Debug.Log(videoFiles.result[0].isHorizontalSlider + "  " + numberOfVideos);

        SetVideoManager();


        for (int i = 0; i < numberOfVideos; i++)
        {

            VideoPlayers[i].prepareCompleted -= DoTheseOncePrepared;
            VideoPlayers[i].prepareCompleted += DoTheseOncePrepared;
            VideoPlayers[i].Prepare();
        }


        

    }

    public void Play()
    {
        if(isReady || VideoPlayers[0].isPaused)
        {
            for (int i=0; i<numberOfVideos;i++)
            {
                VideoPlayers[i].Play();
            }
            isPlaying = true;

        }

    }
    private void Stop()
    {
        if (isReady && VideoPlayers[0].isPlaying)
        {
            for (int i = 0; i < numberOfVideos; i++)
            {
                VideoPlayers[i].Stop();
            }
            isPlaying = false;

        }

    }
    public void Pause()
    {
        if (isReady && VideoPlayers[0].isPlaying)
        {
            for (int i = 0; i < numberOfVideos; i++)
            {
                VideoPlayers[i].Pause();
            }

        }
        isPlaying = false;

    }

    //bool resumePlaying = false;
    public void Seek(float location)
    {
        if (isReady)
        {
            //resumePlaying = isPlaying;

            //if(resumePlaying)
            //{
            //    Pause();

            //}

            float tempF = location * (float)VideoPlayers[0].length;

            //Debug.Log(" seek to: " + tempF);
            //Debug.Log("current time:  " + VideoPlayers[0].time);

            for (int i = 0; i < numberOfVideos; i++)
            {
                VideoPlayers[i].time = tempF;

            }
            //Play();
            //Pause();


        }
        else
        {
            Debug.Log("Not Ready");
        }

    }



    private IEnumerator SkipAframeNcontinue()
    {
        yield return new WaitForSeconds(0.5f);

        Debug.Log("Playing");
        Play();
    }

    private void CheckIfReady()
    {
        isReady = false;
        if(numberOfVideos<=3)
        {
            //if(AreVideosOfSameAspectRatio())
            //{
                if (AreVideosNotNullOrEmpty())
                {
                    if (AreVideosEqualLength())
                    {
                        Debug.Log("Videos Equal Length");
                        isReady = true;
                    }
                    else
                    {

                        SetErrorTexture();
                        Debug.Log("Error: Videos Unequal Length");
                        isReady = false;
                    }
                }
                else
                {

                    SetErrorTexture();
                    isReady = false;
                }

            //}
            //else
            //{
            //    Debug.Log("Not same aspect ratios");
            //    SetErrorTexture();
            //    isReady = false;
            //}


        }
        else
        {
            SetErrorTexture();
            isReady = false;
            Debug.Log("Error: Maximum videos allowed=3");
        }

    }

    private bool AreVideosOfSameAspectRatio()
    {
        float AR = 0;
        for(int i=0; i<numberOfVideos;i++)
        {
            AR = Mathf.Abs(AR + ((float)VideoPlayers[i].texture.width / VideoPlayers[i].texture.height));
            Debug.Log("AR:  " + AR);
        }
        AR = AR / numberOfVideos;
        Debug.Log("Delta aspect:  " + Mathf.Abs(AR - aspectRatio));
        if(Mathf.Abs(AR-aspectRatio)<=0.0001)
        {
            return true;
        }
        else
        {
            return false;
        }


    }

    private void SetErrorTexture()
    {
        Stop();
        for (int i = 0; i < 3; i++)
        {
            //VideoPlayers[i].url = "";
            VideoPlayers[i].Stop();
            videoRawImages[i].texture = Texture2D.blackTexture;
        }
    }

    private bool AreVideosEqualLength()
    {
        bool tempBool = false;

        switch (numberOfVideos)
        {
            case 1:
                tempBool = true; 
                break;
            case 2:
                if (Mathf.Abs((float)VideoPlayers[0].length - (float)VideoPlayers[1].length)<0.1)
                {

                    Debug.Log("Length:   " + ((float)VideoPlayers[0].length));
                    tempBool = true;
                }

                Debug.Log("Two Videos :  "+ Mathf.Abs((float)VideoPlayers[0].length - (float)VideoPlayers[1].length));
                break;
            case 3:
                if (Mathf.Abs((float)VideoPlayers[0].length - (float)VideoPlayers[1].length) < 0.1 && Mathf.Abs((float)VideoPlayers[1].length - (float)VideoPlayers[2].length) < 0.1)
                {

                    tempBool = true;
                }
                Debug.Log("Three Videos");
                break;
   
            default:
                Debug.Log("Error: No videos found");
                break;
        }

        return tempBool;
    }

    private bool AreVideosNotNullOrEmpty()
    {
        bool tempBool = true; 

        for(int i=0;i<numberOfVideos;i++)
        {
            if(string.IsNullOrEmpty(VideoPlayers[i].url))
            {
                tempBool = false;
            }
        }

        return tempBool;
    }

    private int VideosLoopCompleted = 0;

    private void OnVideoEndReached(UnityEngine.Video.VideoPlayer vp)
    {

        VideosLoopCompleted++;
        if(isPlaying && VideosLoopCompleted==numberOfVideos)
        {

            Seek(0);

            Debug.Log("Number of Videos completed : " +VideosLoopCompleted);
            VideosLoopCompleted = 0;
        }
    }


}

[System.Serializable]
public class VideoFiles
{
    public Video[] result;

    public VideoFiles(Video[] tempIn)
    {
        result = tempIn;
    }
}


[System.Serializable]
public class Video
{
    public string name;
    public bool isHorizontalSlider;
    public string tag;

    public Video(string s1, bool b1, string s2)
    {
        name = s1;
        isHorizontalSlider = b1;
        tag = s2;
    }
}

