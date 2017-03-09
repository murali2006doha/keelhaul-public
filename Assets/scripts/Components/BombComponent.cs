using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class BombComponent : MonoBehaviour {

    BombControllerComponent parentCannon;
    PlayerInput player;

    int blinks = 5;
    float blinkTime;

    public GameObject largeBombZone;
    public GameObject smallBombZone;
    public GameObject bombModel;

    //these are all the animations involved
    public float explosion_duration = 1.5f;
    public float damage;
    public float waitTimeToExplode;
    public AudioClip fuseSound;
    public AudioClip explosionSound;


    void Start() {

        blinkTime = (waitTimeToExplode / (float) blinks);
        transform.rotation = Quaternion.Euler(-90, 0, 0); //prefabs should be adjusted so this is no longer necc

    }


    internal void Initialize(BombControllerComponent parent, PlayerInput input) {
        this.parentCannon = parent;
        this.player = input;
    }


    public PlayerInput getPlayer() {
        return player;
    }

    [PunRPC]
    public void DestroySelf() {
        Destroy(this.gameObject);
    }


    void startSound(AudioClip sound) {
        this.gameObject.AddComponent<SoundInitializer> ().soundClip = sound;
    }

    public IEnumerator ActivateBomb() {

        startSound (fuseSound);
        smallBombZone.SetActive (false);
        largeBombZone.SetActive (true);

        for(int i = 0; i < blinks; i++){ //blinks when activated
            largeBombZone.GetComponent<Renderer> ().material.color = Color.white;
            bombModel.GetComponent<Renderer> ().material.color = Color.white;
            yield return new WaitForSeconds(blinkTime);
            largeBombZone.GetComponent<Renderer> ().material.color = Color.yellow;
            bombModel.GetComponent<Renderer> ().material.color = Color.red;
            yield return new WaitForSeconds(blinkTime);
        }

        //explode!
        Destroy (largeBombZone);            //destroy the parameter zone

        if (GetComponent<PhotonView>().isMine) {
            startSound (explosionSound);
            GameObject exp = explode(); //produces an explosion
                                        //Invoke ("fadeHalo", .5f);
            yield return new WaitForSeconds(explosion_duration);

            Destroy(exp);                   //destroy the explosion
        }
        
    }

    [PunRPC]
    public void RPCActivateBomb()
    {
        StartCoroutine(ActivateBomb());
    }



    void OnTriggerEnter(Collider other) {
        if (!GetComponent<PhotonView>().isMine)
        {
            return;
        }

        if ((other.gameObject.name).Equals ("playerMesh") && 
            player.gameObject != other.GetComponentInParent<PlayerInput>().gameObject) {//to activate a bomb

 
            if (parentCannon.getBombList().Contains (other.gameObject) == false) {
                player.gameStats.numOfBombsDetonated += 0.5f;
                GetComponent<PhotonView>().RPC("RPCActivateBomb", PhotonTargets.All);
            }
        }

        if (other.gameObject.name.Contains("Kraken")) {
            KrakenInput kraken = other.gameObject.GetComponent<KrakenInput> ();
            if (kraken.submerged == false) { //only if not submerged
                kraken.gameStats.numOfBombsDetonated++;
                StartCoroutine(ActivateBomb());
            }
        }

    }

 
    private GameObject explode(){
        if (gameObject != null)
        {
            Destroy(gameObject);
        }

        if (!GetComponent<PhotonView>().isMine) {
            return null;
        }

        GameObject exp = PhotonNetwork.Instantiate(PathVariables.bombExplosionPath, gameObject.transform.position, Quaternion.identity, 0);
        exp.GetComponent<PhotonView>().RPC("Initialize", PhotonTargets.All, this.damage, this.player.GetId());
        GetComponent<PhotonView>().RPC("DestroySelf", PhotonTargets.All);
        return exp;
    }


}
