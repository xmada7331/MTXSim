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
        target.SetActive(true);
        target.transform.SetAsFirstSibling();
        MailIcon.sprite = MailAlert;
    }

    public void ResetMail()
    {
        MailIcon.sprite = DefaultAlert;
    }

    public void ReadMail(Image target)
    {
        target.color = new Color32(255, 255, 255, 50);
    }
}
