using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCursor : MonoBehaviour
{
    [SerializeField]
    private Camera cam;
    [SerializeField]
    private Transform player;
    [SerializeField]
    private float threshold;

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        Vector3 targetPos = (player.position + mousePos) / 2;
        
        targetPos.x = Mathf.Clamp(targetPos.x, -threshold + player.position.x, threshold + player.position.x);
        targetPos.y = Mathf.Clamp(targetPos.y, -threshold + player.position.y, threshold + player.position.y);

        this.transform.position = targetPos;
    }
}
