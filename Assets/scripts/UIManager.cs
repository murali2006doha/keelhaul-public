using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using DentedPixel;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private const float SCORE_ANIMATION_TIME = 2f;
    public Text bombs;
    public Slider bombBar;
    public Slider healthBar;
    public Slider enemyIslandHealthBar;
    public Slider submergeBar;
    public Text points;

    //ship exclamation
    public GameObject shipAlert;
    public float alertTime;
    //public Slider altFireBar;
    public Slider altFireBar;
    public Slider scoreBar;
    public GameObject scorePosition;
    public GameObject scoreDestinationPosition;
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
    float enemyHealth = 3f;
    int wobbleCount = 0;
    public Slider boostBar;
    public SpriteRenderer barrelTooltip;
    Barrel barrelObj;
    public Vector3 tooltipOffset;
    public TutorialUIManager tutorialManager;
    public bool enableTutorials = false;
    public GameObject finishText;
    public GameObject colorTint;
    public GameObject worldSpace;
	public StatsModal statsModal;
    Image boostBarPanel;
	bool x = true;
    private int playerNum = 1;
    private bool isShip = false;

    public GameObject killText;
    string temp;
    public GameObject fadePanel;

    public UIAnimationManager animManager;

    [SerializeField] Image characterPortrait;
    [SerializeField] Image characterPortraitBackground;

    bool highlight = true;
    ShipEnum shipType;
    public TMPro.TextMeshProUGUI killFeed;

    public void Initialize(int playerNum, bool isShip, ShipEnum shipType)
    {
        this.shipType = shipType;
        this.isShip = isShip;
        this.playerNum = playerNum;
        this.animManager.Initialize(this.SetPortraitPath, shipType);
        this.InitializePortraitIcons();
    }

    private void SetPortraitPath(string portraitPath, string backgroundPath) {
        this.characterPortrait.sprite = Resources.Load<Sprite>(portraitPath);
      
    }
    void Start()
    {
		statsModal.gameObject.SetActive (false);
        
        boostBarPanel = boostBar.transform.GetChild(0).GetComponentInChildren<Image>();
        tutorialManager.enabled = false;
        resizeFont();
    }

    public void InitializeBarrel()
    {
        print("Rat");
        barrelObj = FindObjectOfType<Barrel>();
        barrelPos = barrelObj.transform.position;
        if (compassArrow != null)
        {
            arrowImage = compassArrow.GetComponent<Image>();
            targetBarrel();
        }
    }


    public void InitializePortraitIcons() {
        this.characterPortrait.sprite = Resources.Load<Sprite>(PathVariables.GetAssociatedPortraitPath(this.shipType));
        this.characterPortraitBackground.sprite = Resources.Load<Sprite>(PathVariables.GetAssociatedPortraitBackgroundPath(this.shipType));
    }
    private void resizeFont()
    {
        var texts = GetComponentsInChildren<Text>();
        foreach (Text text in texts)
        {
            text.fontSize *= 2;
        }
    }

    void Update()
    {
        setEnemyIslandBar();
    }

    public void updatePoint(int point)
    {
        points.text = (point).ToString();
        spawnScoreAnim();
        if (isShip) {
            this.animManager.OnScore();
        }
    }

    private void spawnScoreAnim()
    {

        GameObject instantiated = ((GameObject)(Instantiate(scoreAnimation)));

        instantiated.transform.parent = this.transform.GetChild(0);
        instantiated.GetComponent<RectTransform>().localPosition = scorePosition.GetComponent<RectTransform>().localPosition;
        instantiated.GetComponent<RectTransform>().localRotation = scorePosition.GetComponent<RectTransform>().localRotation;
        instantiated.GetComponent<RectTransform>().localScale = scorePosition.GetComponent<RectTransform>().localScale;


        LeanTween.move(instantiated, scoreDestinationPosition.transform.position, SCORE_ANIMATION_TIME);
        LeanTween.alphaCanvas(instantiated.GetComponent<CanvasGroup>(), 0f, SCORE_ANIMATION_TIME);
        LeanTween.scale(instantiated, scoreDestinationPosition.transform.localScale, SCORE_ANIMATION_TIME);

        //instantiated.GetComponent<RectTransform> ().rotation= scorePosition.GetComponent<RectTransform> ().rotation;
    }


    public void decrementEnemyHealth()
    {
        enemyHealth = enemyIslandHealthBar.value - (enemyIslandHealthBar.maxValue / 3.0f);
        spawnScoreAnim();
    }


    public void setEnemyIslandBar()
    {
        if (enemyIslandHealthBar != null)
        {
            float step = GlobalVariables.uiSliderSpeed * Time.deltaTime; //Lerp Speed

            enemyIslandHealthBar.value = Mathf.MoveTowards(enemyIslandHealthBar.value, enemyHealth, step);
        }
    }

    public void triggerShipAlert()
    {
        if (!shipAlert.activeSelf)
        {
            shipAlert.SetActive(true);
            Invoke("disableShipAlert", alertTime);
        }
    }

    void disableShipAlert()
    {
        shipAlert.SetActive(false);
    }

    public int decrementPoint()
    {
        int point = Mathf.Max(int.Parse(points.text) - 1, 0);
        points.text = (point).ToString();
        return point;
    }


    public void setSubmergeBar(float breath)
    {
        submergeBar.value = breath;
    }

    public void setHealthBar(float health)
    {
        float step = GlobalVariables.uiSliderSpeed * Time.deltaTime; //Lerp Speed

        healthBar.value = Mathf.MoveTowards(healthBar.value, health, step);

    }

    public void updateShipUI(Vector3 pos, bool hittingBarrel)
    {
        updateCompass(pos);
        if (hittingBarrel && barrelTooltip != null)
        {
            barrelTooltip.enabled = true;
            barrelTooltip.transform.position = barrelObj.transform.position + tooltipOffset;

        }
        else
        {
            barrelTooltip.enabled = false;
        }
        if (boostBar.value < 1f)
        {
            float step = Time.deltaTime / 6f; //Todo: take from ship
            boostBar.value = Mathf.MoveTowards(boostBar.value, 1f, step);

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
        if (compassArrow != null && arrowTarget != null)
        {
            arrowTargetPos = arrowTarget.transform.position;
            Vector3 difference = arrowTargetPos - pos;
            float sign = (arrowTargetPos.z < pos.z) ? -1.0f : 1.0f;
            float angle = Vector3.Angle(Vector2.right, difference) * sign;
            float sinVal = Mathf.Sin(wobbleSpeed * Time.time * GlobalVariables.gameSpeed);
            if (sinVal > 0.00001 || sinVal < -0.00001)
            {
                wobbleCount++;
            }
            if (wobbleCount == 10)
            {
                changeWobbleIntensity();
            }
            compassArrow.transform.rotation = Quaternion.Euler(MathHelper.setZ(Vector3.zero, angle - 50 + sinVal * wobbleIntensity));
        }
    }


    public void changeCompassColor(Color color)
    {
        if (compassArrow != null && arrowImage.color != color)
        {
            arrowImage.color = color;
        }
    }


    public void setBoostBar(float value)
    {
        boostBar.value = value;
    }


    public void setBoostBar(float value,bool disabled)
    {
        this.setBoostBar(value);
        if (disabled)
        {
            boostBarPanel.color = Color.gray;
        }
        else
        {
            boostBarPanel.color = Color.white;
        }
    }


    public void setScoreBar(float score)
    {
        if (scoreBar != null)
        {
            scoreBar.value = score;
        }
    }


    public void setBombBar(float score)
    {
        if (bombBar != null)
        {
            bombBar.value = score;
        }
    }


    public int decrementBomb()
    {
        if (int.Parse(bombs.text) > 0)
        {
            int bomb = int.Parse(bombs.text) - 1;
            bombs.text = (bomb).ToString();
            return bomb;
        }

        else
        {
            int bomb = 0;
            bombs.text = (bomb).ToString();
            return bomb;
        }
    }


    public void resetBomb()
    {
        int bomb = 3;
        bombs.text = (bomb).ToString();
        bombBar.value = 1.0f;
    }

    public void resetAltFireMeter()
    {

        altFireBar.value = 0;
        //altfireFill.GetComponent<Image> ().fillAmount = 0;

    }

    public void setAltFireMeter(float value)
    {
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
        changeCompassColor(Color.green);
        this.arrowTarget = target;
    }

    void changeWobbleIntensity()
    {
        currentIntensity = UnityEngine.Random.Range(2, wobbleIntensity * 5);
        wobbleCount = 0;
    }

    public void activateFinishAndColorTint()
    {
        hideDeathAnimation();
        colorTint.SetActive(true);
        activateFinishText();
    }

    public void showDeathAnimation(int player, string ship)
    {
		LeanTween.alphaCanvas(fadePanel.GetComponent<CanvasGroup> (), 1f, 1f);
        killText.SetActive(true);
        var tex = killText.GetComponent<Text>().text;
        temp = tex;
        tex = tex.Replace("r1", "player " + player);
        killText.GetComponent<Text>().text = tex;
    }

    public void hideDeathAnimation()
    {
		LeanTween.alphaCanvas(fadePanel.GetComponent<CanvasGroup> (), 0f, 1f);
        killText.SetActive(false);
        killText.GetComponent<Text>().text = temp;
    }

    void activateFinishText()
    {
        finishText.SetActive(true);
    }

    public void AddToKillFeed(string killer, string killerShip, string victim, string victimShip)
    {
        killFeed.text += "<sprite=\"atlas\" name=\""+ killerShip + "\"> <size=60%>"+killer + "</size> <color=\"red\"> X </color> <sprite=\"atlas\" name=\"" + victimShip + "\" > <size=60%>" +victim +" </size>\n";
        Invoke("RemoveKillFeed", GlobalVariables.killFeedDuration);
    }

    public void RemoveKillFeed()
    {
        killFeed.text = killFeed.text.Substring(killFeed.text.IndexOf("\n")+1);
    }


	public void InitializeStatsScreen(AbstractGameManager gm, PlayerInput input) {
		if (x) {
			statsModal.gameObject.SetActive (true);
			statsModal.InitializeStats ();
			x = false;
		}
	}


	public void SetOffStatsScreen() {
		if (statsModal.gameObject.activeSelf) {
			statsModal.ClearStats ();
			statsModal.gameObject.SetActive (false);
			x = true;
		}
	}

    internal void AddBarrelScoreToKillFeed(string player, string ship)
    {
        killFeed.text += "<sprite=\"atlas\" name=\"" + ship + "\"> <size=60%>" + player + "</size> <color=\"yellow\"> Scored </color>\n";
        Invoke("RemoveKillFeed", GlobalVariables.killFeedDuration);
    }
}