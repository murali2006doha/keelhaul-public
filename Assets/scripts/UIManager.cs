using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

	public Text bombs;
	public Slider bombBar;
	public Slider healthBar;
	public Text points;
	//public Slider altFireBar;
	public Slider altFireBar;
	public Slider scoreBar;
	public GameObject scorePosition;
	public GameObject scoreAnimation;
    public RectTransform compassArrow;
    Vector3 barrel;
    Vector3 arrowTarget;
    Image arrowImage;
    Vector3 compassRotationTarget;
    public float wobbleSpeed = 3.5f;
    float wobbleIntensity = 5f;
    float currentIntensity = 5f;
    int wobbleCount = 0;
    Slider worldSpaceBoost;
    public GameObject boostBar;
    Image boostImg;
    public GameObject boostButtonImg;
    public Vector3 boostOffset;

    bool highlight = true;

    void Start()
    {
        barrel = GameObject.FindObjectOfType<barrel>().transform.position;
        if (compassArrow != null)
        {
            arrowImage = compassArrow.GetComponent<Image>();
            targetBarrel();
        }
        if (boostBar)
        {
            worldSpaceBoost = boostBar.GetComponentInChildren<Slider>();
            boostImg = worldSpaceBoost.fillRect.gameObject.GetComponent<Image>();
            boostButtonImg = worldSpaceBoost.GetComponentInChildren<Animator>().gameObject;
            boostButtonImg.SetActive(false);
        }
    }

    public int incrementPoint() {

		int point = int.Parse (points.text) + 1;
		points.text = (point).ToString();
		Debug.Log (this.transform.GetChild (0));
		GameObject instantiated = ((GameObject)(Instantiate (scoreAnimation)));
		instantiated.transform.parent = this.transform.GetChild(0);
		instantiated.GetComponent<RectTransform> ().localPosition = scorePosition.GetComponent<RectTransform> ().localPosition;
		instantiated.GetComponent<RectTransform> ().localRotation = scorePosition.GetComponent<RectTransform> ().localRotation;
		instantiated.GetComponent<RectTransform> ().localScale = scorePosition.GetComponent<RectTransform> ().localScale;
		//instantiated.GetComponent<RectTransform> ().rotation= scorePosition.GetComponent<RectTransform> ().rotation;

		return point;
	}


	public void setPoint(int i) {

		points.text = (i).ToString();

	}


	public int decrementPoint() {
		int point = Mathf.Max(int.Parse (points.text) - 1,0);
		points.text = (point).ToString();
		return point;
	}


	public void setHealthBar(float health) {
		healthBar.value = health;
	}

    public void updateCompass(Vector3 pos)
    {
        if (compassArrow!=null && arrowTarget!=null)
        {
            Vector3 difference = arrowTarget - pos;
            float sign = (arrowTarget.z < pos.z) ? -1.0f : 1.0f;
            float angle =  Vector3.Angle(Vector2.right, difference) * sign;
            float sinVal = Mathf.Sin(wobbleSpeed * Time.time * GlobalVariables.gameSpeed);
            if(sinVal > 0.00001 || sinVal < -0.00001)
            {
                wobbleCount++;
            }
            if (wobbleCount == 10)
            {
                changeWobbleIntensity();
            }
           compassArrow.transform.rotation = Quaternion.Euler(MathHelper.setZ(Vector3.zero, angle - 50 + sinVal*wobbleIntensity));
        }
    }

    public void updateBoostSlider(Vector3 pos)
    {
        if (worldSpaceBoost) {

            worldSpaceBoost.transform.position = pos+ boostOffset;
            if(worldSpaceBoost.value <= 0.5f)
            {
                if(boostImg.color.a > 0)
                {
                    boostButtonImg.SetActive(false);
                    boostImg.CrossFadeAlpha(0f, 0.5f, false);
                }
            } else if(worldSpaceBoost.value >= 0.8)
            {
               
               boostImg.CrossFadeAlpha(1f, 0.5f, false);
                
            }
            if(worldSpaceBoost.value == worldSpaceBoost.maxValue)
            {
                boostButtonImg.SetActive(false);
                if (highlight)
                {
                    highlight = false;
                    //Invoke("empasize", 2);
                    //Invoke("unflashBoost", 4);
                }
            }
        }

    }

    public void changeCompassColor(Color color)
    {
        if (compassArrow !=null && arrowImage.color!=color)
        {
            arrowImage.color = color;
        }
    }


	public void setworldSpaceBoost(float wind) {
		worldSpaceBoost.value = wind;
	}


	public void setScoreBar(float score) {
		if (scoreBar != null) {
			scoreBar.value = score;
		}
	}


	public void setBombBar(float score) {
		if (bombBar != null) {
			bombBar.value = score;
		}
	}



	public int decrementBomb() {
		if (int.Parse (bombs.text) > 0) {
			int bomb = int.Parse (bombs.text) - 1;
			bombs.text = (bomb).ToString ();
			return bomb;
		}

		else{
			int bomb = 0;
			bombs.text = (bomb).ToString ();
			return bomb;
		}
	}


	public  void resetBomb() {
		int bomb = 3;
		bombs.text = (bomb).ToString ();
		bombBar.value = 1.0f;
	}

	public void resetAltFireMeter() {

		altFireBar.value = 0;
		//altfireFill.GetComponent<Image> ().fillAmount = 0;

	}

	public void setAltFireMeter(float value) {
		altFireBar.value = value;
		//altfireFill.GetComponent<Image> ().fillAmount = value;
	}

    public void targetBarrel()
    {
        this.arrowTarget = barrel;
        changeCompassColor(Color.yellow);
    }

    public void setTarget(Vector3 target)
    {
        this.arrowTarget = target;
    }

    void changeWobbleIntensity()
    {
        currentIntensity = Random.Range(2, wobbleIntensity*5);
        wobbleCount = 0;
    }

}