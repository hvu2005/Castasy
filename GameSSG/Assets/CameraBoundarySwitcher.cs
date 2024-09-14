using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraBoundarySwitcher : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera vcam;
    [SerializeField] private CinemachineConfiner2D confiner;
    [SerializeField] private Collider2D boundaryFromLeft;
    [SerializeField] private Collider2D boundaryFromRight;
    [SerializeField] private Cat cat;
    [SerializeField] private Collider2D currentBoundary;
    [SerializeField] private Image fadeScreen;
    [SerializeField] private float fadeDuration = 1f;
    private bool isTransitioning = false;

    private void Start()
    {
        if (confiner != null && currentBoundary != null)
        {
            confiner.m_BoundingShape2D = currentBoundary;
            Debug.Log("Initial boundary set to: " + currentBoundary.name);
        }
    }

    public void MoveToNewBoundary(Collider2D left, Collider2D right)
    {
        if (!isTransitioning)
        {
            if (cat.body.velocity.x > 0f && currentBoundary == left)
            {
                StartCoroutine(TransitionToBoundary(right));
            }
            else if (cat.body.velocity.x < 0f && currentBoundary == right)
            {
                StartCoroutine(TransitionToBoundary(left));
            }
        }
    }

    private IEnumerator TransitionToBoundary(Collider2D newBoundary)
    {
        isTransitioning = true;


       

        cat.DisableMovement();
        if (cat.isFacingRight)
        {
            cat.body.velocity += new Vector2(6f, 0f);
        }
        else
        {
            cat.body.velocity += new Vector2(-6f, 0f);
        }

        yield return new WaitForSeconds(0.2f);
        cat.DisableMovement();

        confiner.m_BoundingShape2D = newBoundary;
        currentBoundary = newBoundary;
        Debug.Log("Boundary switched to: " + newBoundary.name);

        yield return new WaitForSeconds(0);
        


        cat.EnableMovement();

        isTransitioning = false;
    }

    private IEnumerator Fade(float targetAlpha)
    {
        float startAlpha = fadeScreen.color.a;
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / fadeDuration);
            fadeScreen.color = new Color(fadeScreen.color.r, fadeScreen.color.g, fadeScreen.color.b, newAlpha);
            yield return null;
        }

        
        fadeScreen.color = new Color(fadeScreen.color.r, fadeScreen.color.g, fadeScreen.color.b, targetAlpha);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (currentBoundary == null)
            {
                if (cat.body.velocity.x > 0f)
                {
                    currentBoundary = boundaryFromLeft;
                }
                else if (cat.body.velocity.x < 0f)
                {
                    currentBoundary = boundaryFromRight;
                }
            }

            Debug.Log("Trigger fired by: " + other.name);
            MoveToNewBoundary(boundaryFromLeft, boundaryFromRight);
        }
    }
}


