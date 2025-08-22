using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.Windows.WebCam;
using static UnityEngine.UIElements.UxmlAttributeDescription;

public class CameraManager : MonoBehaviour
{
    [SerializeField] VideoCapture m_VideoCapture = null;


    [SerializeField] private int participant_num = 100;
    [SerializeField] private int session = 1;

    private void Start()
    {
        
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            Debug.Log("sada");
            if (m_VideoCapture != null) StartRecord(participant_num, session);
        }
        else if (Input.GetKeyDown(KeyCode.C)) 
        {
            StopRecord();
        }
    }
    public void StartRecord(int participant, int session)
    {
        participant_num = participant;
        this.session = session;

        VideoCapture.CreateAsync(false, OnVideoCaptureCreated);
    }

    public void StopRecord()
    {
        StopRecordingVideo();
    }
    void OnVideoCaptureCreated(VideoCapture videoCapture)
    {
        if (videoCapture != null)
        {
            m_VideoCapture = videoCapture;

            Resolution cameraResolution = VideoCapture.SupportedResolutions.OrderByDescending((res) => res.width * res.height).First();
            float cameraFramerate = VideoCapture.GetSupportedFrameRatesForResolution(cameraResolution).OrderByDescending((fps) => fps).First();

            CameraParameters cameraParameters = new CameraParameters();
            cameraParameters.hologramOpacity = 0.0f;

            /*cameraParameters.frameRate = 10;
            cameraParameters.cameraResolutionWidth = 640;
            cameraParameters.cameraResolutionHeight = 480;*/

            cameraParameters.frameRate = cameraFramerate;
            cameraParameters.cameraResolutionWidth = cameraResolution.width;
            cameraParameters.cameraResolutionHeight = cameraResolution.height;
            cameraParameters.pixelFormat = CapturePixelFormat.BGRA32;

            m_VideoCapture.StartVideoModeAsync(cameraParameters,
                                                VideoCapture.AudioState.None,
                                                OnStartedVideoCaptureMode);
        }
        else
        {
            Debug.LogError("Failed to create VideoCapture Instance!");
        }
    }

    void OnStartedVideoCaptureMode(VideoCapture.VideoCaptureResult result)
    {
        if (result.success)
        {
            string filename = string.Format("session_{0}.mp4", session);
            string folder = participant_num.ToString();
            string filepath = @"C:\Users\Tal\Desktop\Experiment\Records\";

            m_VideoCapture.StartRecordingAsync(filepath, OnStartedRecordingVideo);
        }
    }

    void OnStartedRecordingVideo(VideoCapture.VideoCaptureResult result)
    {
        Debug.Log("Started Recording Video!");
        // We will stop the video from recording via other input such as a timer or a tap, etc.
    }

    // The user has indicated to stop recording
    void StopRecordingVideo()
    {
        m_VideoCapture.StopRecordingAsync(OnStoppedRecordingVideo);
    }

    void OnStoppedRecordingVideo(VideoCapture.VideoCaptureResult result)
    {
        Debug.Log("Stopped Recording Video!");
        m_VideoCapture.StopVideoModeAsync(OnStoppedVideoCaptureMode);
    }

    void OnStoppedVideoCaptureMode(VideoCapture.VideoCaptureResult result)
    {
        m_VideoCapture.Dispose();
        m_VideoCapture = null;
    }

}
