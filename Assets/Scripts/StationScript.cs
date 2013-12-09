using UnityEngine;
using System.Collections;

public class OutOfAmmo : BaseEvent { }

public class StationScript : MonoBehaviour {

    public Sprite[] ammoSprites;
    private bool m_destroyed = false;
    private int m_ammo = 10;

    public int ammo { get { return m_ammo; } }
    public bool destroyed { get { return m_destroyed; } }

    public void DisableStation() {
        renderer.enabled = false;
        collider2D.enabled = false;
        m_destroyed = true;
        m_ammo = 0;
        SetStationSprite();
    }

    public void ResetStation() {
        renderer.enabled = true;
        collider2D.enabled = true;
        m_destroyed = false;
        m_ammo = 10;
        SetStationSprite();
    }

    private void SetStationSprite() {
        GetComponent<SpriteRenderer>().sprite = ammoSprites[m_ammo];
    }

    public void OnStationFired() {
        m_ammo--;
        if (m_ammo <= 0) {
            EventManager.instance.QueueEvent(new OutOfAmmo());
            m_ammo = 0;
        }
        SetStationSprite();
    }
}
