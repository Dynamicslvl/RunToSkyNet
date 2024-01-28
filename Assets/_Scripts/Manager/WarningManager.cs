using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dasis.UI;

public class WarningManager : MonoBehaviour
{
    public RectTransform rect;
    public Player player;
    public List<Enemy> enemies;
    public List<Transform> warns;

    private float xMin, xMax, yMin, yMax;
    private const float maxRadius = 20;
    private const float offset = 1;

    private void Update()
    {
        GetBounds();
        for (int i = 0; i < enemies.Count; i++)
        {
            Vector2 direction = enemies[i].XYPosition - player.XYPosition;
            Vector2 boundDir = GetDirectionVector(direction.normalized);
            warns[i].gameObject.SetActive(direction.magnitude > boundDir.magnitude);
            warns[i].position = player.XYPosition + boundDir;
        }
    }

    public void GetBounds()
    {
        Vector3[] corners = UIToolkits.CornersOfRect(rect);
        xMin = corners[0].x + offset;
        xMax = corners[3].x - offset;
        yMin = corners[0].y + offset;
        yMax = corners[1].y - offset;
    }

    public Vector2 GetDirectionVector(Vector2 direction)
    {
        int e = 1000;
        int l = 0, r = e, mid = 0;
        while (l <= r)
        {
            mid = (l + r) / 2;
            if (l == r) break;
            if (IsOutOfBound(mid * maxRadius / e * direction))
            {
                r = mid - 1;
            }
            else
            {
                l = mid + 1;
            }
        }
        return mid * maxRadius / e * direction;
    }

    public bool IsOutOfBound(Vector2 dirVec)
    {
        Vector2 pos = player.XYPosition + dirVec;
        if (pos.x < xMin || pos.x > xMax) return true;
        if (pos.y < yMin || pos.y > yMax) return true;
        return false;
    }
}
