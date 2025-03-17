using UnityEngine;

public class ClearPlayerPrefs : MonoBehaviour
{
    [ContextMenu("Clear PlayerPrefs")]
    public void ClearPlayerPrefsData()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        Debug.Log("PlayerPrefs data cleared.");
    }
}