using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorFollowLauncher : MonoBehaviour
{
    public GameObject screenCenterPos;
    public LineRenderer lineRenderer;
    private Canvas myCanvas;
    private CanvasGroup cg;
    public bool ready = true;

    // Start is called before the first frame update
    void Start()
    {
        myCanvas = GetComponentInParent<Canvas>();
        cg = GetComponent<CanvasGroup>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePosition();
    }

    private void UpdatePosition()
    {
        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(myCanvas.transform as RectTransform, Input.mousePosition, myCanvas.worldCamera, out pos);
        transform.position = myCanvas.transform.TransformPoint(pos);

        //more graphics here
        cg.alpha = ready ? 1f : 0.25f;
    }

    private void LateUpdate()
    {
        lineRenderer.gameObject.SetActive(ready);
        if (ready)
        {
            DrawLine();
        }
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
