using UnityEngine;
using UnityEngine.UI;


public class BulletTime : MonoBehaviour
{
    public Camera m_OrthographicCamera; // The main camera

    float m_ViewPositionX, m_ViewPositionY, m_ViewWidth, m_ViewHeight; // The camera's X and Y positions    The camera's Width and Height values

    public Image bTimeTint; // The bullet time camera tint

    public Image bTimeBar; // The ability bar

    public Image bTimeBarFill; // The fill to the ability bar

    public GameManagerScript gameManager;

    [Range(0, 1)]
    public float smoothTime; // How fast bullet time takes to get to it's lowest values

    [Range(4, 6)]
    public float btimecamsize; // How much the camera zooms in

    public float bTimeSpeed; // How slow bullet time is

    public bool bulletTime; // If bullet time is active

    [Range(3, 7)]
    public float maxBTimeLength; // The max value of time you have to use bullet time

    public float bTimeLength; // The current value of time you have left to use bullet time

    public float minBTimeLength; // If bTimeLength reaches this value, the bar is empty

    public float fillamount; // The current fill amount of the ability bar

    public bool canUseBTime; // If the player can use bullet time

    public bool canUseDash; // If the player can use dash

    public bool isHolding; // If the ability button is being held

    public bool justDashed; // If the player just used their dash

    public Color a, b; // a = minAlpha  b = maxAlpha

    public PlayerMovementWithDash player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        canUseDash = player.enableDash; // Finds if the player script enabled the dash ability

        Color c = bTimeTint.color; // Sets c to the bullet time tints color
        c.a = 0; // The bullet time tint's removed
        bTimeTint.color = c; // Sets the bullet time tints color to c

        bulletTime = false; // Bullet time is deactivated
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.scifiworld)
        {
            canUseBTime = true;
            canUseDash = false;
        }
        else if (gameManager.fantasyworld)
        {
            canUseBTime = false;
            canUseDash = true;
        }

            GetCurrentFill();

        #region BUTTON CHECK
        // If ability button is held down, isHolding is true. If not, it's false
        if (Input.GetButtonDown("Fire2") && canUseBTime || Input.GetButtonDown("Fire2") && canUseDash)
        {
            isHolding = true;
        }
        else if (Input.GetButtonUp("Fire2") && canUseBTime || Input.GetButtonUp("Fire2") && canUseDash)
        {
            isHolding = false;
        }
        #endregion

        #region BULLET TIME CHECK
        // Bullet Time is activated when the ability button is held, the player can use bullet time, and the ability bar isn't empty
        if (isHolding && canUseBTime && bTimeLength > 0)
        {
            bulletTime = true;
            bTime();
        }
        else // The bar fills up, the camera zooms out, bullet time is false, and the tints alpha is set to 0
        {
            bTimeLength += Time.deltaTime;
            Time.timeScale = Mathf.Lerp(Time.timeScale, 1f, smoothTime);
            m_OrthographicCamera.orthographicSize = Mathf.Lerp(m_OrthographicCamera.orthographicSize, 7, smoothTime);
            bulletTime = false;

            bTimeTint.color = Color.Lerp(bTimeTint.color, a, smoothTime);
        }
        
        // If the bar reaches the max value, it's set to the max value so it doesn't go higher than it should
        if (bTimeLength > maxBTimeLength)
        {
            bTimeLength = maxBTimeLength;
        }

        // If the ability button is held while the bar is empty, the bar can't refill until the player releases the button
        if ((bTimeLength <= minBTimeLength) && isHolding)
        {
            bulletTime = false;
            isHolding = false;
            bTimeLength = minBTimeLength;
        }
        #endregion

        if ((bTimeLength > 0.3f) && canUseDash && isHolding)
        {
        }
    }

    #region BULLET TIME METHOD
    public void bTime()
    {
        // If bullet time is active
        if (bulletTime)
        {
            m_OrthographicCamera.orthographicSize = Mathf.Lerp(m_OrthographicCamera.orthographicSize, btimecamsize, smoothTime); // The camera zooms in from it's current size, to the size it should be during bullet time, by smoothTime
            Time.timeScale = Mathf.Lerp(Time.timeScale, bTimeSpeed, smoothTime); // Time slows down from it's current value, to bullet time speed, by smoothTime
            Color c = bTimeTint.color; // c is the color of the bullet time tint panel
            c.a = 0.2f; // The bullet time tints max alpha is set to 0.2
            bTimeTint.color = Color.Lerp(bTimeTint.color, b, smoothTime); // The tints alpha increases from it's current value, to it's max value, by smoothTime

            bTimeLength -= Time.deltaTime / Time.timeScale; // The bar decreases by Time.timeScale, devided by itself, so that it keeps at the same rate in secs
        }
    }

    #endregion

    #region ABILITY BAR FILL METHOD
    [ExecuteInEditMode()]
    void GetCurrentFill()
    {
        float currentOffset = bTimeLength - minBTimeLength;
        float maxOffset = maxBTimeLength - minBTimeLength;

        fillamount = (float)bTimeLength / (float)maxBTimeLength;
        bTimeBarFill.fillAmount = fillamount;
    }
    #endregion
}
