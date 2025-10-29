using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UFOlight : MonoBehaviour
{
    [Range(0, 1)] public float fill = 0f;
    public float speed = 0.8f;
    Material mat;
    void Awake() { mat = GetComponent<Renderer>().material; } // SpriteRenderer 或 MeshRenderer
    void Update()
    {
        fill = Mathf.MoveTowards(fill, 1f, speed * Time.deltaTime);
        mat.SetFloat("_Fill", fill);
    }
}
