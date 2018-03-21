using UnityEngine;
using System.Collections;

public class TextureBillboard : MonoBehaviour
{
    Camera m_Camera;

    private void Awake()
    {
        m_Camera = Camera.main;
    }

    void Update()
    {
        transform.LookAt(transform.position + m_Camera.transform.rotation * Vector3.forward,
            m_Camera.transform.rotation * Vector3.up);
    }

    private void LateUpdate()
    {
        transform.eulerAngles = new Vector3(90, 0, 180);
    }
}
