using UnityEngine;
using System.Collections;

public class EarthHealthBar : MonoBehaviour, IEventListener {

    public Sprite[] healthArray;
    private int currentSprite = 0;

    public bool HandleEvent(IEvent evt) { return false; }

    void Update() {
        if (GameManager.instance.earthHP != currentSprite) {
            currentSprite = GameManager.instance.earthHP;
            SpriteRenderer sprRend = GetComponent<SpriteRenderer>();
            sprRend.sprite = healthArray[currentSprite];
        }
    }
}
