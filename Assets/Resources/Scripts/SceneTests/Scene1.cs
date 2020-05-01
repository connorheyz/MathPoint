using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scene1 : MonoBehaviour
{
    Variable time = new Variable(0f, 't');
    Vector v = new Vector(Vector2.up, Vector2.zero);
    CartesianVectorSpace space1;
    CartesianVectorSpace space2;
    public Dictionary<string, Color> colors;
    public float timer;

    void Start()
    {
        space1 = CartesianVectorSpace.CreateCartesianVectorSpace(new Rect(-175, 0, 300, 300), 50f, Vector2.right, Vector2.up, Vector2.zero, new Vector2(-20, 20), new Vector2(-20, 20));
        space2 = CartesianVectorSpace.CreateCartesianVectorSpace(new Rect(175, 0, 300, 300), 50f, Vector2.right, Vector2.one, Vector2.zero, new Vector2(-20, 20), new Vector2(-20, 20));
        space1.DrawVector(v, false, true);
        space2.DrawVector(v, false, true);
        v.SetFunctions(time, t => Mathf.Cos(t), t => Mathf.Sin(t)*2);
    }

    private void Update()
    {
        float dt = Time.deltaTime;
        time.value += dt;
        timer += dt;
        if (timer > 6f)
        {
            space2.LinearTransform(new Vector2(Random.Range(-2, 2), Random.Range(-2, 2)), new Vector2(Random.Range(-2, 2), Random.Range(-2, 2)), false);
            timer = 0f;
        }
    }
}
