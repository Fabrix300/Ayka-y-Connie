using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCameraController : MonoBehaviour
{
    private Transform aykaTransform;
    private Transform cameraTransform;
    public Vector3 offset;
    public float smoothFactor;

    void Start()
    {
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        aykaTransform = GameObject.Find("Ayka").transform;
        cameraTransform = Camera.main.transform;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 targetPosition = aykaTransform.transform.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(cameraTransform.position, targetPosition, smoothFactor * Time.fixedDeltaTime);
        cameraTransform.position = new Vector3(smoothedPosition.x, smoothedPosition.y, cameraTransform.position.z);
    }
}
