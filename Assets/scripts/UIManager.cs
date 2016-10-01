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
    Vector3 barrelPos;
    GameObject arrowTarget;
    Vector3 arrowTargetPos;
    Image arrowImage;
    Vector3 compassRotationTarget;
    public float wobbleSpeed = 3.5f;
    float wobbleIntensity = 5f;
    float currentIntensity = 5f;
    int wobbleCount = 0;
    public Slider boostBar;
    public SpriteRenderer barrelTooltip;
    barrel barrelObj;
    public Vector3 tooltipOffset;
    public TutorialUIManager tutorialManager;
    public bool enableTutorials = true;


    bool highlight = true;

    void Start()
    {
        barrelObj = GameObject.FindObjectOfType<barrel>();
        barrelPos = barrelObj.transform.position;
        if (compassArrow != null)
        {
            arrowImage = compassArrow.GetComponent<Image>();
            targetBarrel();
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

    public void updateShipUI(Vector3 pos, bool hittingBarrel)
    {
        updateCompass(pos);
        if (hittingBarrel && barrelTooltip!=null)
        {
            barrelTooltip.enabled = true;
            barrelTooltip.transform.position = barrelObj.transform.position + tooltipOffset;

        }
        else
        {
            barrelTooltip.enabled = false;
        }
          
    }

    public void updateTutorialPrompts(Camera cam, PlayerActions input)
    {
        if (enableTutorials && tutorialManager != null && !tutorialManager.isEmpty())
        {
            tutorialManager.updateTutorial(cam, input);
        }
    }

    public void updateCompass(Vector3 pos)
    {
        if (compassArrow!=null && arrowTarget!=null)
        {
            arrowTargetPos = arrowTarget.transform.position;
            Vector3 difference = arrowTargetPos - pos;
            float sign = (arrowTargetPos.z < pos.z) ? -1.0f : 1.0f;
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


    public void changeCompassColor(Color color)
    {
        if (compassArrow !=null && arrowImage.color!=color)
        {
            arrowImage.color = color;
        }
    }


	public void setBoostBar(float value) {
		boostBar.value = value;
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
        this.arrowTarget = barrelObj.gameObject;
        changeCompassColor(Color.yellow);
    }

    public void setTarget(GameObject target)
    {
        this.arrowTarget = target;
    }

    void changeWobbleIntensity()
    {
        currentIntensity = Random.Range(2, wobbleIntensity*5);
        wobbleCount = 0;
    }

}