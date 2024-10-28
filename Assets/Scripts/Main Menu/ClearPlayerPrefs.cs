using UnityEngine;

public class ClearPlayerPrefs : MonoBehaviour
{
    void Awake()
    {
        // Clear all PlayerPrefs data
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
    }

    void Start()
    {
        // Clear PlayerPrefs again in Start, if needed
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
    }
}

