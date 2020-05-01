using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scene1 : MonoBehaviour
{
    Variable time = new Variable(0f, 't');
    Vector i = new Vector(Vector2.up, Vector2.zero);
    Vector j = new Vector(Vector2.right, Vector2.zero);
    CartesianVectorSpace space1;
    CartesianVectorSpace space2;
    public List<Color> colors;
    public float timer;

    void Start()
    {
        space1 = CartesianVectorSpace.CreateCartesianVectorSpace(new Rect(-400, 0, 700, 700), 100f, Vector2.right, Vector2.up, Vector2.zero, new Vector2(-20, 20), new Vector2(-20, 20));
        space2 = CartesianVectorSpace.CreateCartesianVectorSpace(new Rect(400, 0, 700, 700), 100f, Vector2.right, Vector2.one, Vector2.zero, new Vector2(-20, 20), new Vector2(-20, 20),1.5f);
        space1.DrawVector(i, colors[0], true, true);
        space2.DrawVector(i, colors[0], true, true);
        space1.DrawVector(j, colors[1], true, true);
        space2.DrawVector(j, colors[1], true, true);
    }

    private void Update()
    {
        float dt = Time.deltaTime;
        time.value += dt;
        timer += dt;
        if (timer > 6f)
        {
            space2.LinearTransform(new Vector2(Random.Range(-2, 2), Random.Range(-2, 2)), new Vector2(Random.Range(-2, 2), Random.Range(-2, 2)), true);
            timer = 0f;
        }
    }
}
