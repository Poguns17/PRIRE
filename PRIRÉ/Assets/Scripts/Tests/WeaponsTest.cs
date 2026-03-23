using UnityEngine;

public class WeaponsTest : MonoBehaviour 
{
    private Pistol pistol;

    private void Start()
    {
        pistol = GetComponent<Pistol>();
    
    }

    private void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            pistol.Shoot();
            Debug.Log("Ammo: " + pistol.AmmoCount);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            pistol.Reload();
            Debug.Log("Reloading...");
        }
    }

    private void OnGUI()
    {
        float size = 10f;
        float x = Screen.width / 2 - size / 2;
        float y = Screen.height / 2 - size / 2;
        GUI.Box(new Rect(x, y, size, size), "");
        
    }
}
