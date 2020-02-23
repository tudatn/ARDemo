using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARTrackedImageManager))]
public class TrackedImageInfoManager : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The camera to set on the world space UI canvas for each instantiated image info.")]
    Camera m_WorldSpaceCanvasCamera;

    ARTrackedImageManager m_TrackedImageManager;
    private void Awake()
    {
        m_TrackedImageManager = GetComponent<ARTrackedImageManager>();
    }

    private void OnEnable()
    {
        m_TrackedImageManager.trackedImagesChanged += OnTrackedImagesChanged;
    }

    private void OnDisable()
    {
        m_TrackedImageManager.trackedImagesChanged -= OnTrackedImagesChanged;
    }

    void UpdateInfo(ARTrackedImage trackedImage)
    {
        // set canvas camera
        var canvas = trackedImage.GetComponentInChildren<Canvas>();
        canvas.worldCamera = m_WorldSpaceCanvasCamera;

        if (trackedImage.trackingState != TrackingState.None)
        {
            trackedImage.transform.localScale = new Vector3(trackedImage.size.x, 1f, trackedImage.size.y);
            trackedImage.transform.Translate(trackedImage.size.x + 0.01f, 0, 0);
        }
    }

    void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        foreach (var trackedImage in eventArgs.added)
        {
            UpdateInfo(trackedImage);
        }

        foreach (var trackedImage in eventArgs.updated)
        {
            UpdateInfo(trackedImage);
        }
    }
}
