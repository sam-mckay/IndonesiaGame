using UnityEngine;
using System.Collections;

public class PU_Distance : PowerUp
{
    public PU_Distance(Vector3 _position, float _abilityStrength, PowerUp_Controller _controller) : base(_position, _abilityStrength, _controller)
    {

    }
	
    public override void OnTriggerEnter(Collider other)
    {
        if(other.tag == Tags.player)
        {
            other.GetComponent<Player>().maxDistance += abilityStrength;
            Destroy();
        }
    }
}
