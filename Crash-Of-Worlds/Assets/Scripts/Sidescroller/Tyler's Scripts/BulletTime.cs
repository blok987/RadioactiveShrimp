using UnityEngine;
using UnityEngine.UI;

public class BulletTime : MonoBehaviour
{
    public Camera m_OrthographicCamera;

    float m_ViewPositionX, m_ViewPositionY, m_ViewWidth, m_ViewHeight;

    public Image bTimeTint;

    [Range(0, 1)]
    public float smoothTime; // How fast bullet time takes to get to it's lowest

    [Range(4, 6)]
    public float btimecamsize;

    public float bTimeSpeed; // How slow bullet time is

    public bool bulletTime; // If bullet time is active

    public Color a, b;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Color c = bTimeTint.color;
        c.a = 0;
        bTimeTint.color = c;

        bulletTime = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Fire2")) // Bullet Time when right mouse button is clicked
        {
            bTime();
        }
        else
        {
            Time.timeScale = Mathf.Lerp(Time.timeScale, 1f, smoothTime);
            m_OrthographicCamera.orthographicSize = Mathf.Lerp(m_OrthographicCamera.orthographicSize, 7, smoothTime);
            bulletTime = false;

            bTimeTint.color = Color.Lerp(bTimeTint.color, a, smoothTime);
        }
    }

    public void bTime()
    {
        bulletTime = true;
        m_OrthographicCamera.orthographicSize = Mathf.Lerp(m_OrthographicCamera.orthographicSize, btimecamsize, smoothTime);
        Time.timeScale = Mathf.Lerp(Time.timeScale, bTimeSpeed, smoothTime);
        Color c = bTimeTint.color;
        c.a = 0.2f;
        bTimeTint.color = Color.Lerp(bTimeTint.color, b, smoothTime);
    }
}
