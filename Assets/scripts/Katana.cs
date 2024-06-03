using Photon.Pun;
using UnityEngine;

[CreateAssetMenu(fileName = "Katana", menuName = "Other/Katana")]
public class Katana : Item

{
    public float damage;
    public ParticleSystem particles;
    public float knockbackAmount;
    public float shotRange = 2f;
    private RaycastHit2D hit;
    public float hitkb;
    PhotonView shooterView;
    public override void Use(bool isRight, Transform gunTip, PhotonView view)
    {
        shooterView = view;
        GameObject player = view.gameObject;
        player.GetComponent<Movement>().Jump();
    }

    public override float GetRecoilKb()
    {
        return knockbackAmount;
    }
}
