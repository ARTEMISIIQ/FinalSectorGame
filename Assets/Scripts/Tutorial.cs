using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Tutorial : MonoBehaviour
{
    [SerializeField]
    private bool usedForward;
    [SerializeField]
    private bool usedBackwards;
    [SerializeField]
    private bool usedLeft;
    [SerializeField]
    private bool usedRight;
    [SerializeField]
    private bool usedBrake;
    [SerializeField]
    private bool usedEscape;
    [SerializeField]
    private bool usedReset;

    public TextMeshProUGUI tutorialText;
    // Start is called before the first frame update
    void Start()
    {
        usedForward = false;
        usedBackwards = false;
        usedLeft = false;
        usedRight = false;
        usedBrake = false;
        usedEscape = false;
        usedReset = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            usedForward = true;
        }
        if (!usedForward)
        {
            tutorialText.text = "Press W or the Up arrow key to accelerate forwards. Passing the GREEN ARCH will start your timer.";
        }
        else
        {

            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            {
                usedBackwards = true;
            }
            if (!usedBackwards)
            {
                tutorialText.text = "Press S or the Down arrow key to accelerate backwards (REVERSE)";
            }
            else
            {
                if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
                {
                    usedLeft = true;
                }
                if (!usedLeft)
                {
                    tutorialText.text = "Press A or the Left arrow key to rotate the car towards the left";
                }
                else
                {

                    if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
                    {
                        usedRight = true;
                    }
                    if (!usedRight)
                    {
                        tutorialText.text = "Press D or the Right arrow key to rotate the car towards the Right";
                    }
                    else
                    {
                        if (Input.GetKey(KeyCode.Space))
                        {
                            usedBrake = true;
                        }
                        if (!usedBrake)
                        {
                            tutorialText.text = "Press Space to decelerate your car (BRAKING). TIP: Brake to steer better on sharp turns (Try to turn at that curve)";
                        }
                        else
                        {
                            if (Input.GetKey(KeyCode.Escape))
                            {
                                usedEscape = true;
                            }
                            if (!usedEscape)
                            {
                                tutorialText.text = "Press Esc to access settings";
                            }
                            else
                            {
                                if (Input.GetKey(KeyCode.R))
                                {
                                    usedReset = true;
                                }
                                if (!usedReset)
                                {
                                    tutorialText.text = "Go through that checkpoint (The Yellow one)! Then, press R to return to your last checkpoint or start";
                                }
                                else
                                {
                                    tutorialText.text = "You have completed the tutorial! Go through the red END GATEWAY. Try to complete eachtrack as FAST as possible!";
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
