using UnityEngine;
using UnityEngine.UI;

public class ParallaxBackground : MonoBehaviour
{
    public RawImage Sky, Cloud, Islands;
    [Range(0.0f, 0.02f)]
    public float SkySpeed, CloudSpeed, IslandSpeed;

    private void Update()
    {
        Vector2 currentPosition = Camera.main.transform.position;

        Sky.uvRect = new Rect(new Vector2(currentPosition.x * SkySpeed, 0), Vector2.one);
        Cloud.uvRect = new Rect(new Vector2(currentPosition.x * CloudSpeed, 0), Vector2.one);
        Islands.uvRect = new Rect(new Vector2(currentPosition.x * IslandSpeed, 0), Vector2.one);
    }

}
