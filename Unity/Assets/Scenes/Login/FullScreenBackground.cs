using UnityEngine;

public class FullScreenBackground : MonoBehaviour {

    #region Variables

    float cameraHeight;
    Vector2 cameraSize;
    Vector2 spriteSize;

    #endregion

    #region Start/Update
    void Start () {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

        cameraHeight = Camera.main.orthographicSize * 2;
        cameraSize = new Vector2(Camera.main.aspect * cameraHeight, cameraHeight);
        spriteSize = spriteRenderer.sprite.bounds.size;

        ScaleBackground();
    }
	
	void Update () {
	
	}

    #endregion

    #region Method

    public void ScaleBackground()
    {
        Vector2 scale = transform.localScale;
        //if (cameraSize.x >= cameraSize.y)
        //{   // Landscape (or equal)
        //    scale *= cameraSize.x / spriteSize.x;
        //}
        //else
        //{   // Portrait
        //    scale *= cameraSize.y / spriteSize.y;
        //}
        scale *= cameraSize.y / spriteSize.y;

        //transform.position = Vector2.zero; // Optional
        transform.localScale = scale;
    }

    #endregion
}
