
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    public Transform target;

    public Vector3 offset;

    public float smoothSpeed = 0.125f;

    public bool YMaxEnabled = false;
    public float Ymax;
    public bool YMinEnabled = false;
    public float Ymin;

    public bool XMaxEnabled = false;
    public float Xmax;
    public bool XMinEnabled = false;
    public float Xmin;
    

    void FixedUpdate()
    {

       Vector3 targetPosition = target.position + offset;

        if (YMaxEnabled && YMinEnabled)
        {
            targetPosition.y = Mathf.Clamp(target.position.y, Ymin, Ymax);
        }
        else if (YMaxEnabled)
        {
            targetPosition.y = Mathf.Clamp(target.position.y, target.position.y, Ymax);
        }
        else if (YMinEnabled)
        {
            targetPosition.y = Mathf.Clamp(target.position.y, Ymin, target.position.y);
        }


        if (XMaxEnabled && XMinEnabled)
        {
            targetPosition.x = Mathf.Clamp(target.position.x, Xmin, Xmax);
        }
        else if (XMaxEnabled)
        {
            targetPosition.x = Mathf.Clamp(target.position.x, target.position.x, Xmax);
        }
        else if (XMinEnabled)
        {
            targetPosition.x = Mathf.Clamp(target.position.x, Xmin, target.position.x);
        }

        Vector3 smoothPosition = Vector3.Lerp(transform.position, targetPosition, smoothSpeed);

        transform.position = smoothPosition;

       
    }

}
