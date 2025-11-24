using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class CameraMovement : MonoBehaviour
{
    [Range(0, 1)]
    public float smoothTime;

    public Transform playerTransform;

    public Camera m_OrthographicCamera;

    public TopDownMovement playerMove;
    private Animation cam;

    public void Update()
    {
        Camera cam = GetComponent<Camera>();
    }

    public void FixedUpdate()
    {
        Vector3 pos = GetComponent<Transform>().position;

        pos.x = Mathf.Lerp(pos.x, playerTransform.position.x, smoothTime);
        pos.y = Mathf.Lerp(pos.y, playerTransform.position.y, smoothTime);

        GetComponent<Transform>().position = pos;
    }

}
