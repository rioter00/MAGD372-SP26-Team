using UnityEngine;
using UnityEngine.UI;

public class Windshield_Overlay : MonoBehaviour
{
    public Texture2D[] oilFrames;
    public Texture2D[] dirtFrames;
    public Texture2D[] snowFrames;
    public bool isOil;
    public bool isDirt;
    public bool isSnow;

    public LaneMover_PlayerOnly laneMover;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        

        this.GetComponent<RawImage>().enabled = false;
        this.GetComponent<RawImage>().color = Color.white;
        if (isOil)
        {
            if (laneMover.oil_ANIMATION_Frame > 0)
            {
                this.GetComponent<RawImage>().color = Color.black;
                this.GetComponent<RawImage>().enabled = true;
                if (laneMover.oil_ANIMATION_Frame > 7)
                {
                    this.GetComponent<RawImage>().texture = oilFrames[6];
                    return;
                }
                else
                {
                    this.GetComponent<RawImage>().texture = oilFrames[laneMover.oil_ANIMATION_Frame - 1];
                    return;
                }

            }
        }

        if (isDirt)
        {
            if (laneMover.dirt_ANIMATION_Frame > 0)
            {
                this.GetComponent<RawImage>().enabled = true;
                if (laneMover.dirt_ANIMATION_Frame > 6)
                {
                    this.GetComponent<RawImage>().texture = dirtFrames[5];
                    return;
                }
                else
                {
                    this.GetComponent<RawImage>().texture = dirtFrames[laneMover.dirt_ANIMATION_Frame - 1];
                    return;
                }
            }
        }


        if (isSnow)
        {
            if (laneMover.snow_ANIMATION_Frame > 0)
            {
                this.GetComponent<RawImage>().enabled = true;
                if (laneMover.snow_ANIMATION_Frame > 5)
                {
                    this.GetComponent<RawImage>().texture = snowFrames[4];
                    return;
                }
                else
                {
                    this.GetComponent<RawImage>().texture = snowFrames[laneMover.snow_ANIMATION_Frame - 1];
                    return;
                }
            }
        }
        
    }
}
