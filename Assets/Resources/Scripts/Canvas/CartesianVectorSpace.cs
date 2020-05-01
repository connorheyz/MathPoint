using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pixelplacement;
using UnityEngine.UI;

public class CartesianVectorSpace : MonoBehaviour, ICanvas
{
    public Vector2 iHat;
    public Vector2 jHat;
    public Rect frame;
    public float zoom;
    public Vector2 xRange;
    public Vector2 yRange;
    public Vector2 origin;
    public Color axisColor;
    public Color mainColor;
    GameObject axisPrefab;
    GameObject vectorPrefab;
    public static GameObject planePrefab;
    public Transform xAxis;
    public Transform yAxis;
    public float animationSpeed;
    private AnimationCurve animationCurve;
    public delegate void VectorSpaceEventHandler();
    public event VectorSpaceEventHandler OnFinishTransform;
    public Dictionary<Vector, RectTransform> vectorBodies = new Dictionary<Vector, RectTransform>();

    public void ActivateOnFinishTransform()
    {
        if (OnFinishTransform != null)
        {
            OnFinishTransform();
        }
    }
    
    void Start()
    {
        InitializeAxes();
    }

    public static CartesianVectorSpace CreateCartesianVectorSpace(Rect position, float zoom, Vector2 i, Vector2 j, Vector2 origin, Vector2 xr, Vector2 yr, float animationSpeed = 3f)
    {
        if (planePrefab == null)
        {
            planePrefab = Resources.Load<GameObject>("Prefabs/Plot");
        }
        CartesianVectorSpace space = Instantiate(planePrefab, FindObjectOfType<Canvas>().transform).GetComponent<CartesianVectorSpace>();
        space.frame = position;
        space.zoom = zoom;
        space.iHat = i;
        space.jHat = j;
        space.xRange = xr;
        space.yRange = yr;
        space.origin = origin;
        space.animationSpeed = animationSpeed;
        space.animationCurve = Tween.EaseInOutStrong;
        space.axisPrefab = Resources.Load<GameObject>("Prefabs/AxisLine");
        space.vectorPrefab = Resources.Load<GameObject>("Prefabs/Vector");
        return space;
    }

    public void LinearTransform(Vector2 newI, Vector2 newJ, bool isAnimated = true)
    {
        float xAngle = Mathf.Atan2(newI.y, newI.x) * Mathf.Rad2Deg;
        float yAngle = Mathf.Atan2(newJ.y, newJ.x) * Mathf.Rad2Deg;
        Vector2 xSpacing = newJ * zoom;
        Vector2 ySpacing = newI * zoom;
        foreach (RectTransform r in xAxis.GetComponentsInChildren<RectTransform>())
        {
            if (r.gameObject.name != "AxisX")
            {
                int i = int.Parse(r.gameObject.name);
                bool isNumeric = int.TryParse(r.name, out i);
                if (isAnimated)
                {
                    Tween.LocalPosition(r, origin + (xSpacing * i), animationSpeed, 0f, animationCurve);
                    Tween.Size(r, new Vector2(newJ.magnitude * zoom * (yRange.y - yRange.x), 1), animationSpeed, 0f, animationCurve);
                    Tween.Rotation(r, new Vector3(0, 0, xAngle), animationSpeed, 0f, animationCurve);
                } else
                {
                    r.localPosition = origin + (xSpacing * i);
                    r.sizeDelta = new Vector2(newJ.magnitude * zoom * (yRange.y - yRange.x), 1);
                    r.localEulerAngles = new Vector3(0, 0, xAngle);
                }
            }
        }
        foreach (RectTransform r in yAxis.GetComponentsInChildren<RectTransform>())
        {
            if (r.gameObject.name != "AxisY")
            {
                int i = int.Parse(r.gameObject.name);
                if (isAnimated)
                {
                    Tween.LocalPosition(r, origin + (ySpacing * i), animationSpeed, 0f, animationCurve);
                    Tween.Size(r, new Vector2(newJ.magnitude * zoom * (xRange.y - xRange.x), 1), animationSpeed, 0f, animationCurve);
                    Tween.Rotation(r, new Vector3(0, 0, yAngle), animationSpeed, 0f, animationCurve);
                }
                else
                {
                    r.localPosition = origin + (ySpacing * i);
                    r.sizeDelta = new Vector2(newJ.magnitude * zoom * (xRange.y - xRange.x), 1);
                    r.localEulerAngles = new Vector3(0, 0, yAngle);
                }
            }
        }
        iHat = newI;
        jHat = newJ;
        UpdateDrawings();
    }

    void UpdateDrawings()
    {
        UpdateVectorBodies();
    }

    void UpdateVectorBodies()
    {
        foreach (KeyValuePair<Vector, RectTransform> entry in vectorBodies)
        {
            entry.Key.MoveTo(entry.Key.position);
        }
    }

    void InitializeAxes()
    {
        GetComponent<RectTransform>().localPosition = frame.position;
        Tween.Size(GetComponent<RectTransform>(), frame.size, 1f, 0f, animationCurve);
        float xAngle = Mathf.Atan2(iHat.y, iHat.x) * Mathf.Rad2Deg;
        float yAngle = Mathf.Atan2(jHat.y, jHat.x) * Mathf.Rad2Deg;
        Vector2 xSpacing = jHat * zoom;
        Vector2 ySpacing = iHat * zoom;
        for (int i = (int)xRange.x; i < xRange.y; i++)
        {
            RectTransform axis = Instantiate(axisPrefab, xAxis).GetComponent<RectTransform>();
            axis.gameObject.name = i.ToString();
            axis.localPosition = origin + (xSpacing * i);
            Tween.Size(axis, new Vector2(jHat.magnitude*zoom*(yRange.y - yRange.x), 1), animationSpeed, 0f, animationCurve);
            axis.localEulerAngles = new Vector3(0, 0, xAngle);
            if (i == 0)
            {
                axis.GetComponent<Image>().color = mainColor;
            } else
            {
                axis.GetComponent<Image>().color = axisColor;
            }
        }
        for (int k = (int)yRange.x; k < yRange.y; k++)
        {
            RectTransform axis = Instantiate(axisPrefab, yAxis).GetComponent<RectTransform>();
            axis.gameObject.name = k.ToString();
            axis.localPosition = origin + (ySpacing * k);
            Tween.Size(axis, new Vector2(iHat.magnitude * zoom * (xRange.y - xRange.x), 1), animationSpeed, 0f, animationCurve);
            axis.localEulerAngles = new Vector3(0, 0, yAngle);
            if (k == 0)
            {
                axis.GetComponent<Image>().color = mainColor;
            }
            else
            {
                axis.GetComponent<Image>().color = axisColor;
            }
        }
    }

    public void DrawVector(Vector vector, Color c, bool isAnimated = true, bool isLocal = true)
    {
        RectTransform r = Instantiate(vectorPrefab, transform).GetComponent<RectTransform>();
        foreach (Image i in r.GetComponentsInChildren<Image>())
        {
            i.color = c;
        }
        vectorBodies[vector] = r;
        Vector2 tVector = Vector2.zero;
        if (isLocal)
        {
            tVector = (iHat * vector.point.x) + (jHat * vector.point.y);
        }
        else
        {
            tVector = vector.point;
        }
        float angle = Mathf.Atan2(tVector.y, tVector.x) * Mathf.Rad2Deg;
        r.localEulerAngles = new Vector3(0, 0, angle);
        r.localPosition = (origin + ((vector.position.x * iHat) + (vector.position.y * jHat))) * zoom;
        if (isAnimated)
        {
            Tween.Size(r, new Vector2((tVector.magnitude * zoom) - 20, r.sizeDelta.y), animationSpeed, 0f, animationCurve);
        } else
        {
            r.sizeDelta = new Vector2((tVector.magnitude * zoom) - 20, r.sizeDelta.y);
        }
        vector.OnMove += (Vector v) => UpdateVector(v, isAnimated, isLocal);
        vector.OnPoint += (Vector v) => UpdateVector(v, isAnimated, isLocal);
    }

    public void UpdateVector(Vector vector, bool isAnimated = true, bool isLocal = true)
    {
        if (vectorBodies[vector])
        {
            RectTransform r = vectorBodies[vector];
            Vector2 tVector = Vector2.zero;
            if (isLocal)
            {
                tVector = (iHat * vector.point.x) + (jHat * vector.point.y);
            } else
            {
                tVector = vector.point;
            }
            float angle = Mathf.Atan2(tVector.y, tVector.x) * Mathf.Rad2Deg;
            if (isAnimated) {
                Tween.LocalPosition(r, (origin + ((vector.position.x * iHat) + (vector.position.y * jHat))) * zoom, animationSpeed, 0f, animationCurve);
                Tween.Rotation(r, new Vector3(0, 0, angle), animationSpeed, 0f, animationCurve);
                Tween.Size(r, new Vector2((tVector.magnitude * zoom) - 20, r.sizeDelta.y), animationSpeed, 0f, animationCurve);
            } else {
                r.localPosition = (origin + ((vector.position.x * iHat) + (vector.position.y * jHat))) * zoom;
                r.localEulerAngles = new Vector3(0, 0, angle);
                r.sizeDelta = new Vector2((tVector.magnitude * zoom) - 20, r.sizeDelta.y);
            }
        }
    }

    public void Clear()
    {
        
    }

    public void DrawText(string text, Rect position, float fontsize = 12)
    {
        throw new System.NotImplementedException();
    }

    public void Destroy()
    {
        throw new System.NotImplementedException();
    }
}
