using UnityEngine;
using System.Collections;

public class StationScript : MonoBehaviour {

    public Sprite[] ammoSprites;
    private int m_ammo = 10;

    public int ammo { get { return m_ammo; } }

    public void OnStationFired() {
        m_ammo--;
        if (m_ammo < 0) m_ammo = 0;
        GetComponent<SpriteRenderer>().sprite = ammoSprites[m_ammo];
    }
}
