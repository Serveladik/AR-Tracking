using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.ARFoundation;

public class ImageTracking : MonoBehaviour
{
    [SerializeField] GameObject[] objectToPlace; 

    Dictionary<string, GameObject> spawnedObjects = new Dictionary<string, GameObject>();
    ARTrackedImageManager trackedImageManager;

    void OnEnable()
    {
        trackedImageManager.trackedImagesChanged += ImageChanged;
    }

    void OnDisable()
    {
        trackedImageManager.trackedImagesChanged -= ImageChanged;
    }

    void Awake()
    {
        trackedImageManager = GetComponent<ARTrackedImageManager>();
        foreach(GameObject prefab in objectToPlace)
        {
            GameObject newObject = Instantiate(prefab, Vector3.zero, Quaternion.identity);
            newObject.name = prefab.name;
            spawnedObjects.Add(prefab.name, newObject);
        }
    }

    void ImageChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        foreach(ARTrackedImage trackedImage in eventArgs.added)
        {
            UpdateImage(trackedImage);
            Debug.Log("Added");
        }
        foreach(ARTrackedImage trackedImage in eventArgs.updated)
        {
            UpdateImage(trackedImage);
            Debug.Log("Updated");
        }
        foreach(ARTrackedImage trackedImage in eventArgs.removed)
        {
            spawnedObjects[trackedImage.name].SetActive(false);
            Debug.Log("Removed");
        }
    }

    void UpdateImage(ARTrackedImage trackedImage)
    {
        Vector3 imagePosition = trackedImage.transform.position;

        GameObject prefab = spawnedObjects[trackedImage.referenceImage.name];
        prefab.transform.position = imagePosition;
        prefab.SetActive(true);

        foreach(GameObject go in spawnedObjects.Values)
        {
            if(go.name != trackedImage.referenceImage.name)
            {
                go.SetActive(false);
            }
        }
    }

}
