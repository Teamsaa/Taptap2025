using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class MouseClick : MonoBehaviour
{
    public SpriteRenderer cat;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, 100, 1 << 6);
            Debug.Log(hit.collider.gameObject.name);
            
            if (hit.collider.gameObject.name == "Cat" && !(Level1.Instance.isPlayingTimeLine))
            {
                StartCoroutine(OnCatClick());
            }
            else if (hit.collider.gameObject.name == "Chair" && !(Level1.Instance.isPlayingTimeLine))
            {
                
            }
            
        }
    }

    IEnumerator OnCatClick()
    {
        Debug.Log("OnCatClick");
        UpdateImage("images/Cat/Cat2", cat);

        yield return new WaitForSeconds(3);
        UpdateImage("images/Cat/Cat1", cat);
    }
    void UpdateImage(string imagePath, SpriteRenderer sr)
    {
        Sprite sprite2 = Resources.Load<Sprite>(imagePath);
        if (sprite2 != null)
        {
            sr.sprite = sprite2;
            Debug.Log("Õº∆¨º”‘ÿ≥…π¶" + sprite2);

        }
        else
        {
            Debug.LogError("Õº∆¨º”‘ÿ¥ÌŒÛ");
        }
    }

    void OnTriggerEnter(Collider coll)
    {

    }
}
