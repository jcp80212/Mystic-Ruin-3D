using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Character
{
    [RequireComponent(typeof(Image))]
    public class PlayerHealthBar : MonoBehaviour
    {
        Image healthBarImage;
        PlayerControl player;

        // Use this for initialization
        void Start()
        {
            player = FindObjectOfType<PlayerControl>();
            healthBarImage = GetComponent<Image>();
        }

        // Update is called once per frame
        void Update()
        {
            healthBarImage.fillAmount = player.GetComponent<HealthSystem>().healthAsPercentage;
        }
    }
}
