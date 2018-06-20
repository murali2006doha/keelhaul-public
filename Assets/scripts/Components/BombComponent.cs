using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class BombComponent : MonoBehaviour {

    BombControllerComponent parentCannon;
    PlayerInput player;
  AbstractGameManager manager;

    int blinks = 5;
    float blinkTime;

    public GameObject largeBombZone;
    public GameObject bombModel;

    //these are all the animations involved
    public float explosion_duration = 1.5f;
    public float damage;
    public float waitTimeToExplode;
    public AudioClip fuseSound;
    public AudioClip explosionSound;


    void Start() {

        blinkTime = (waitTimeToExplode / (float) blinks);
        //transform.rotation = Quaternion.Euler(-90, 0, 0); //prefabs should be adjusted so this is no longer necc

    }


  internal void Initialize(BombControllerComponent parent, PlayerInput input, AbstractGameManager manager) {
        this.parentCannon = parent;
        this.player = input;
    this.manager = manager;
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

        int id = other.GetComponentInParent<PlayerInput>().GetId();
        var otherPlayer = manager.getPlayerWithId(id);

        if ((other.gameObject.name).Equals("playerMesh") && 
            (player.gameObject != other.GetComponentInParent<PlayerInput>().gameObject)) {
          if (manager.getNumberOfTeams() > 1 && player.teamNo == otherPlayer.teamNo)
          {
            print(player.teamNo);
            print(otherPlayer.teamNo);
            return;
          } else {
            if (parentCannon.getBombList().Contains (other.gameObject) == false) {
                player.gameStats.numOfBombsDetonated += 0.5f;
                StartCoroutine(ActivateBomb());
            } 
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
