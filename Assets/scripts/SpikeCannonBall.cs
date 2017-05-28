using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeCannonBall : CannonBall
{

    bool makeSpike = true;

    override public void destroySelf()
    {
        if (GetComponent<PhotonView>().isMine)
        {
            if (makeSpike)
            {
                CreateSpike();
            }
            PhotonNetwork.Destroy(GetComponent<PhotonView>());
        }
    }

    public override void CheckSplash()
    {
        if (this.transform.position.y < 0.01f && !splashed)
        {
            Instantiate(splash, transform.position, transform.rotation);
            CreateSpike();
            splashed = true;
            makeSpike = false;
            Invoke("destroySelf", lifeTime);
        }
    }

    private void CreateSpike()
    {
        if (GetComponent<PhotonView>().isMine)
        {
            var spike = PhotonNetwork.Instantiate(PathVariables.spike, transform.position, Quaternion.identity, 0);
            print("test" + spike);
            spike.GetComponent<PhotonView>().RPC("SetSpikeParent", PhotonTargets.All, this.owner.GetComponent<PlayerInput>().GetId());
        }
        
    }


}
