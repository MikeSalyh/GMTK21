using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorFollowLauncher : MonoBehaviour
{
    public GameObject screenCenterPos;
    public LineRenderer lineRenderer;
    private Canvas myCanvas;

    // Start is called before the first frame update
    void Start()
    {
        myCanvas = GetComponentInParent<Canvas>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(myCanvas.transform as RectTransform, Input.mousePosition, myCanvas.worldCamera, out pos);
        transform.position = myCanvas.transform.TransformPoint(pos);

        DrawLine();
    }

    private void LateUpdate()
    {
        DrawLine();
    }

    void DrawLine()
    {
        List<Vector3> pos = new List<Vector3>();
        pos.Add(screenCenterPos.transform.position);
        pos.Add(transform.position);
        lineRenderer.startWidth = 1f;
        lineRenderer.endWidth = 1f;
        lineRenderer.SetPositions(pos.ToArray());
        lineRenderer.useWorldSpace = true;
    }
}
