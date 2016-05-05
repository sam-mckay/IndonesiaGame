using UnityEngine;
using System.Collections;

public class PU_Brightness : PowerUp
{

    public PU_Brightness(Vector3 _position, float _abilityStrength, PowerUp_Controller _controller) : base(_position, _abilityStrength, _controller)
    {

    }

    public override void OnTriggerEnter(Collider other)
    {
        if (other.tag == Tags.player)
        {
            other.GetComponent<Player>().maxBrightness += abilityStrength;
            Destroy();
        }
    }
}