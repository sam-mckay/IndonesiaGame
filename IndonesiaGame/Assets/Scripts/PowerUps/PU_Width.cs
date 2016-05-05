using UnityEngine;
using System.Collections;

public class PU_Width : PowerUp {

    public PU_Width(Vector3 _position, float _abilityStrength, PowerUp_Controller _controller) : base(_position, _abilityStrength, _controller)
    {

    }

    public override void OnTriggerEnter(Collider other)
    {
        if (other.tag == Tags.player)
        {
            other.GetComponent<Player>().maxWidth += abilityStrength;
            Destroy();
        }
    }
}
