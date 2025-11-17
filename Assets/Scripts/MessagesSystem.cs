using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessagesSystem : MonoBehaviour
{
    public List<GameObject> allMessages;
    public Image MailIcon;
    public Sprite DefaultAlert;
    public Sprite MailAlert;
    public AudioSource mailSound;
    public Animator mailAnimation;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SelectMessage(GameObject target)
    {
        foreach (GameObject message in allMessages)
        {
                message.SetActive(false);
        }
        target.SetActive(true);
    }

    public void GetMessage (GameObject target)
    {
        if (!target.activeSelf)
        {
            target.SetActive(true);
            target.transform.SetAsFirstSibling();
            MailIcon.sprite = MailAlert;
            mailSound.Play();
            StartCoroutine(MailAnimation()); 
        }
    }

    public void ResetMail()
    {
        MailIcon.sprite = DefaultAlert;
    }

    public void ReadMail(Image target)
    {
        target.color = new Color32(255, 255, 255, 50);
    }

    private IEnumerator MailAnimation()
    {
        mailAnimation.SetTrigger("GotMail");
        yield return new WaitForSeconds(1f);
        mailAnimation.SetTrigger("Idle");
        mailAnimation.ResetTrigger("GotMail");
    }
}
