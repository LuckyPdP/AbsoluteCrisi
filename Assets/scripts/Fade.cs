using UnityEngine;

public class Fade : MonoBehaviour
{

    public Animator anim;
  //  public GameObject FadeIn;


    void Update()
    {
   //     FadeIn.SetActive(true);

    }



    public void Fadeout()
    {
        anim.Play("FadeOut");
    }
}
