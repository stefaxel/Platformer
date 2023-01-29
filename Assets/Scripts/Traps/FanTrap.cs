using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FanTrap : MonoBehaviour
{
    [SerializeField] float onTime;
    [SerializeField] float offTime;
    [SerializeField] float forceApplied;

    private Animator fanAnimation;

    private bool fanIsActive = true;
    private bool isActive = true;
    private bool fanCanBlowPlayer;

    //[SerializeField] private AudioClip fanAudio;

    private void Start()
    {
        fanAnimation = GetComponent<Animator>();
        StartCoroutine(FanTrapTrigger());
    }

    private void Update()
    {
        if(!fanIsActive)
            StartCoroutine(FanTrapTrigger());
    }

    IEnumerator FanTrapTrigger()
    {
        if (!isActive)
        {
            fanAnimation.SetBool("activated", false);
            fanCanBlowPlayer = false;
            yield return new WaitForSeconds(offTime);
            fanIsActive = true;
            isActive = true;
        }

        if (isActive)
        {
            fanAnimation.SetBool("activated", true);
            //SoundManager.instance.PlaySoundTime(onTime, fanAudio);
            fanCanBlowPlayer = true;
            yield return new WaitForSeconds(onTime);
            isActive = false;
            fanIsActive = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            if (isActive && fanCanBlowPlayer)
            {
                collision.GetComponent<Rigidbody2D>().AddForce(transform.up * forceApplied, ForceMode2D.Impulse);
            }
        }
    }

}
