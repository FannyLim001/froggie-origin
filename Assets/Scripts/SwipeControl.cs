using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwipeControl : MonoBehaviour
{
    public GameObject scrollbar;
    float scroll_pos = 0;
    float[] pos;
    int posisi = 0;
    bool isDragging = false;

    // Start is called before the first frame update
    void Start()
    {
        pos = new float[transform.childCount];
        float distance = 1f / (pos.Length - 1f);
        for (int i = 0; i < pos.Length; i++)
        {
            pos[i] = distance * i;
        }
    }

    public void Next()
    {
        if (posisi < pos.Length - 1)
        {
            posisi += 1;
            scroll_pos = pos[posisi];
        }
    }

    public void Prev()
    {
        if (posisi > 0)
        {
            posisi -= 1;
            scroll_pos = pos[posisi];
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isDragging = true;
            scroll_pos = scrollbar.GetComponent<Scrollbar>().value;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }

        if (isDragging)
        {
            // Update scrollbar value based on mouse drag
            scroll_pos = scrollbar.GetComponent<Scrollbar>().value;
        }
        else
        {
            // Snap to the nearest position when not dragging
            float distance = 1f / (pos.Length - 1f);
            for (int i = 0; i < pos.Length; i++)
            {
                if (scroll_pos < pos[i] + (distance / 2) && scroll_pos > pos[i] - (distance / 2))
                {
                    scrollbar.GetComponent<Scrollbar>().value = Mathf.Lerp(scrollbar.GetComponent<Scrollbar>().value, pos[i], 0.15f);
                    posisi = i;
                }
            }
        }
    }
}
