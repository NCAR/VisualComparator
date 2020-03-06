using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

#if UNITY_STANDALONE
using System.IO;

#endif

namespace Crosstales.FB.Demo
{
    public class CanvasComponentsScript : MonoBehaviour
    {
        public Sprite fullScreenSprite, smallScreenSprite, playSprite, pauseSprite;

        public Button infoButton;
        public Button fullScreenButton;

        public Button pausePlayButton;
        public Text progressText;
        public Text durationText;
        public GameObject debugText;

        public RectTransform progressSliderRT;
        public RectTransform videoPanelRT;
        public RectTransform controlPanelRT;
        public RectTransform sliderAndTextPanel;
        public RectTransform rightPanel;
        public GameObject helpImage;
        public VideoControllerScript VCS;
        private float tempHeight, tempWidth;
        public GameObject[] InfoObjects;
        private bool isHelpPanelEnabled = false;
        public Text copyrightText;
        public Text titleText;
        public GameObject inputPanel;
        public GameObject[] inputPanelTransforms;
        private string directoryPath;
        public GameObject OpenPlayListButton;

        private Event e;
        private void Start()
        {
            // Update copyright year

            copyrightText.text = "Copyright © UCAR " + System.DateTime.Now.Year.ToString();
            titleText.text = "Video Comparator\n" + "v" + Application.version;

            if(VCS.IsImageComparator)
            {
                Camera.main.backgroundColor = new Color(0f, 0f, 0f, 1f);
                controlPanelRT.sizeDelta = new Vector2(0f, 0f);
                controlPanelRT.gameObject.SetActive(false);
                videoPanelRT.sizeDelta = this.GetComponent<RectTransform>().sizeDelta;
            }
            else
            {
#if UNITY_STANDALONE
            Camera.main.backgroundColor = new Color(0.25f,0.25f,0.25f,1f);
            inputPanel.SetActive(true);
            directoryPath = Application.dataPath +Path.AltDirectorySeparatorChar+".."+ Path.AltDirectorySeparatorChar + ".."+ Path.AltDirectorySeparatorChar;
            OpenPlayListButton.SetActive(true);
#elif UNITY_WEBGL

            Camera.main.backgroundColor = new Color(0f, 0f, 0f, 1f);

#endif

            }


        }


        void OnGUI()
        {
            e = Event.current;



        }
            private void Update()
        {
            if(!VCS.IsImageComparator)
            {
                // set the size of the video panel
                tempHeight = GetComponent<RectTransform>().sizeDelta.y - controlPanelRT.sizeDelta.y;
                tempWidth = GetComponent<RectTransform>().sizeDelta.x;

                if (tempHeight * VCS.aspectRatio <= tempWidth)
                {
                    videoPanelRT.sizeDelta = new Vector2(tempHeight * VCS.aspectRatio, tempHeight);
                }
                else
                {
                    videoPanelRT.sizeDelta = new Vector2(tempWidth, tempWidth / VCS.aspectRatio);
                }

                videoPanelRT.anchoredPosition = new Vector2(videoPanelRT.anchoredPosition.x, (videoPanelRT.sizeDelta.y - tempHeight) / 2f);

                progressText.text = (VCS.VideoPlayers[0].clockTime / (60)).ToString("00") + ":" + (VCS.VideoPlayers[0].clockTime).ToString("00");

                durationText.text = ((VCS.duration - VCS.VideoPlayers[0].clockTime) / (60)).ToString("00") + ":" + (VCS.duration - VCS.VideoPlayers[0].clockTime).ToString("00");


                sliderAndTextPanel.sizeDelta = new Vector2(this.GetComponent<RectTransform>().sizeDelta.x - (120 + rightPanel.sizeDelta.x), sliderAndTextPanel.sizeDelta.y);

                progressSliderRT.sizeDelta = new Vector2(sliderAndTextPanel.sizeDelta.x - ((135f * 2f) + 30), progressSliderRT.sizeDelta.y);

                if (!Screen.fullScreen)
                {
                    fullScreenButton.GetComponent<Image>().enabled = true;
                }
                else
                {
                    fullScreenButton.GetComponent<Image>().enabled = false;
                }


#if UNITY_STANDALONE
                if (Input.GetKey(KeyCode.Escape) && Screen.fullScreen)
                {
                    Screen.fullScreen = false;
                }

#endif

                if (e != null)
                {
#if UNITY_STANDALONE || UNITY_EDITOR


                    if ((e.control && Input.GetKeyDown(KeyCode.O)) && !inputPanel.activeInHierarchy)
                    {

                        ActivateInputPanel();
                    }

#endif

                    if (e.control && Input.GetKeyDown(KeyCode.D))
                    {
                        Debug.Log(!debugText.activeInHierarchy);
                        bool tempB = debugText.activeInHierarchy;
                        debugText.SetActive(!tempB);

                        //VCS.Pause();
                    }

                    //Debug.Log("in");


                }

            }

        

        }

        public void ActivateInputPanel()
        {
            if(!inputPanel.activeInHierarchy)
            {

                //Debug.Log("Show panel");
                if (VCS.isPlaying)
                {
                    pausePlayButton.onClick.Invoke();
                }

                inputPanel.SetActive(true);

            }


        }

        public void OnFullScreenButtonPress()
        {
            if (!Screen.fullScreen)
            {
                Screen.fullScreen = true;
            }

        }

        public void OnPausePlayButtonPressed()
        {
            //SpriteState ss = new SpriteState();

            if (VCS.isPlaying)
            {
                VCS.Pause();
                pausePlayButton.image.sprite = playSprite;
                //ss.highlightedSprite = playSprite;
                //ss.pressedSprite = playSprite;
            }
            else
            {
                VCS.Play();
                pausePlayButton.image.sprite = pauseSprite;
                //ss.highlightedSprite = pauseSprite;
                //ss.pressedSprite = pauseSprite;
            }

        }

        public void OnHelpButtonPress()
        {

            if (isHelpPanelEnabled)
            {
                helpImage.SetActive(false);
                isHelpPanelEnabled = false;
            }
            else
            {
                helpImage.SetActive(true);
                isHelpPanelEnabled = true;
            }

        }

        public void OnSliderFlipButtonPress()
        {

            VCS.SwapSliders();
        }

        private float[] sliderValues = new float[2];

        public void OnInfoPanelEnter()
        {
            //int k = 0;
            //for(int i=0; i<4;i++)
            //{
            //    if(VCS.VideoSliders[i].activeInHierarchy)
            //    {
            //        sliderValues[k] = VCS.VideoSliders[i].GetComponent<Slider>().value;
            //        if(VCS.numberOfVideos==3)
            //        {
            //            VCS.VideoSliders[i].GetComponent<Slider>().value = 1-((1f/3f)*((float)k+1));
            //        }
            //        else
            //        {
            //            VCS.VideoSliders[i].GetComponent<Slider>().value = 0.5f;
            //        }
            //        k++;

            //    }

            //}

            if (!isHelpPanelEnabled)
            {
                ToggleInfoObjectsVisibility(true);
            }

        }

        public void OnInfoPanelExit()
        {
            //int k = 0;
            //for (int i = 0; i < 4; i++)
            //{
            //    if (VCS.VideoSliders[i].activeInHierarchy)
            //    {
            //        VCS.VideoSliders[i].GetComponent<Slider>().value = sliderValues[k];
            //        k++;

            //    }

            //}
            ToggleInfoObjectsVisibility(false);
        }

        private void ToggleInfoObjectsVisibility(bool inBool)
        {
            for (int i = 0; i < VCS.numberOfVideos; i++)
            {

                InfoObjects[i].SetActive(inBool);
                InfoObjects[i].transform.GetChild(0).GetComponent<Text>().text = VCS.Tags[i];

            }

        }

        public void InputPanelClosePressed()
        {

            inputPanel.SetActive(false);
        }

        public void inputPanelPlayPressed()
        {
#if UNITY_STANDALONE
            List<Video> videoList = new List<Video>();
            bool areAllVideosNull = true;
            for (int i = 0; i < inputPanelTransforms.Length; i++)
            {
                if (!string.IsNullOrEmpty(inputPanelTransforms[i].transform.GetChild(1).GetComponent<Text>().text))
                {
                    if(System.IO.File.Exists(inputPanelTransforms[i].transform.GetChild(1).GetComponent<Text>().text))
                    {
                        areAllVideosNull = false;
                        videoList.Add(new Video((inputPanelTransforms[i].transform.GetChild(1).GetComponent<Text>().text),
                            inputPanelTransforms[i].transform.GetChild(3).GetComponent<Toggle>().isOn,
                            inputPanelTransforms[i].transform.GetChild(2).GetComponent<InputField>().text));

                    }
                    else
                    {
                        Debug.Log("File Not Found ...");
                        return;
                    }

                }
                else
                {
                    continue;
                }

            }

            if (!areAllVideosNull)
            {
                VCS.ResetPlayer(new VideoFiles(videoList.ToArray()));
                Debug.Log("video list not null");
                pausePlayButton.onClick.Invoke();
            }
            else
            {
                Debug.Log("Video list is null");
                return;
            }

#endif
            Camera.main.backgroundColor = new Color(0f, 0f, 0f, 1f);
            inputPanel.SetActive(false);

        }

        public void OnFileSelectButtonPressed()
        {

#if UNITY_STANDALONE
            string tempStr = FileBrowser.OpenSingleFile("Open single file", directoryPath, new ExtensionFilter("Video Files", "mp4", "mov","m4v","mpeg","ogv","mpg","vp8","webm","dv","avi"));
            

            directoryPath= Path.GetFullPath(tempStr);

            Debug.Log(directoryPath);

            EventSystem.current.currentSelectedGameObject.transform.parent.GetChild(1).GetComponent<Text>().text = tempStr;

            #endif
        }

        public void OnClearButtonPress()
        {
            EventSystem.current.currentSelectedGameObject.transform.parent.GetChild(1).GetComponent<Text>().text = "";
        }



    }
}

