using UnityEngine;
using UnityEngine.UI;


public class BulletTime : MonoBehaviour
{
    public Camera m_OrthographicCamera;

    float m_ViewPositionX, m_ViewPositionY, m_ViewWidth, m_ViewHeight;

    public Image bTimeTint;

    public Image bTimeBar;

    public Image bTimeBarFill;

    [Range(0, 1)]
    public float smoothTime; // How fast bullet time takes to get to it's lowest

    [Range(4, 6)]
    public float btimecamsize;

    public float bTimeSpeed; // How slow bullet time is

    public bool bulletTime; // If bullet time is active

    [Range(3, 7)]
    public float maxBTimeLength;

    public float bTimeLength;

    public float minBTimeLength;

    public float fillamount;

    public bool canUseBTime;

    public bool canUseDash;

    public bool isHolding;

    public bool justDashed;

    public Color a, b;

    public PlayerMovementWithDash player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        canUseDash = player.enableDash;

        Color c = bTimeTint.color;
        c.a = 0;
        bTimeTint.color = c;

        bulletTime = false;
    }

    // Update is called once per frame
    void Update()
    {

        GetCurrentFill();

        if (Input.GetButtonDown("Fire2") && canUseBTime || Input.GetButtonDown("Fire2") && canUseDash)
        {
            isHolding = true;
        }
        else if (Input.GetButtonUp("Fire2") && canUseBTime || Input.GetButtonUp("Fire2") && canUseDash)
        {
            isHolding = false;
        }

        if (isHolding && canUseBTime && bTimeLength > 0) // Bullet Time when right mouse button is clicked
        {
            bulletTime = true;
            bTime();
        }
        else
        {
            bTimeLength += Time.deltaTime;
            Time.timeScale = Mathf.Lerp(Time.timeScale, 1f, smoothTime);
            m_OrthographicCamera.orthographicSize = Mathf.Lerp(m_OrthographicCamera.orthographicSize, 7, smoothTime);
            bulletTime = false;

            bTimeTint.color = Color.Lerp(bTimeTint.color, a, smoothTime);
        }

        if (bTimeLength > maxBTimeLength)
        {
            bTimeLength = maxBTimeLength;
        }

        if ((bTimeLength <= minBTimeLength) && isHolding)
        {
            bulletTime = false;
            isHolding = false;
            bTimeLength = minBTimeLength;
        }

        if ((bTimeLength > 0.3f) && canUseDash && isHolding)
        {
        }
    }

    public void bTime()
    {
        if (bulletTime)
        {
            m_OrthographicCamera.orthographicSize = Mathf.Lerp(m_OrthographicCamera.orthographicSize, btimecamsize, smoothTime);
            Time.timeScale = Mathf.Lerp(Time.timeScale, bTimeSpeed, smoothTime);
            Color c = bTimeTint.color;
            c.a = 0.2f;
            bTimeTint.color = Color.Lerp(bTimeTint.color, b, smoothTime);

            bTimeLength -= Time.deltaTime / Time.timeScale;
        }
    }
    [ExecuteInEditMode()]
    void GetCurrentFill()
    {
        float currentOffset = bTimeLength - minBTimeLength;
        float maxOffset = maxBTimeLength - minBTimeLength;

        fillamount = (float)bTimeLength / (float)maxBTimeLength;
        bTimeBarFill.fillAmount = fillamount;
    }
}
