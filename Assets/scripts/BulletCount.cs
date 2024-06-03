using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;

public class BulletCount : MonoBehaviour
{
    public TextMeshProUGUI text;

    [PunRPC]
    public void updateBulletCount(int count)
    {
        text.text = count.ToString();
    }

}
