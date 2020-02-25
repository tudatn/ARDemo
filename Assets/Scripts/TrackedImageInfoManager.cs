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

    // objects to be shown upon detecting preference images
    [SerializeField]
    GameObject[] m_TrackedObjects;

    // tracked image objects
    private Dictionary<string, GameObject> m_TrackedImages = new Dictionary<string, GameObject>();

    ARTrackedImageManager m_TrackedImageManager;

    private void Awake()
    {
        m_TrackedImageManager = GetComponent<ARTrackedImageManager>();
    }

    private void Start()
    {
        // instantiate tracked image objects
        foreach(GameObject trackedObject in m_TrackedObjects)
        {
            GameObject newObject = Instantiate(trackedObject);
            newObject.SetActive(false);
            newObject.name = trackedObject.name;
            m_TrackedImages.Add(trackedObject.name, newObject);
        }
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
        if (m_TrackedImages.Count > 0)
        {
            GameObject objectToShow = m_TrackedImages[trackedImage.referenceImage.name]; // make sure the reference name = tracked image name
            // set canvas camera
            var canvas = objectToShow.GetComponentInChildren<Canvas>();
            canvas.worldCamera = m_WorldSpaceCanvasCamera;

            // position and scale tracked image
            objectToShow.transform.position = trackedImage.transform.position;
            objectToShow.transform.rotation = trackedImage.transform.rotation;
            objectToShow.transform.Rotate(90f, 0, 0);
            objectToShow.transform.localScale = new Vector3(trackedImage.size.x, trackedImage.size.y, 1);
            objectToShow.transform.Translate(trackedImage.size.x + 0.01f, 0, 0);

            ShowTrackedImageObject(trackedImage.referenceImage.name);
        }

    }

    void ShowTrackedImageObject(string name)
    {
        foreach(GameObject obj in m_TrackedImages.Values)
        {
            obj.SetActive(obj.name == name);
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

        foreach (var trackedImage in eventArgs.removed)
        {
            m_TrackedImages[trackedImage.name].SetActive(false);
        }
    }
}
