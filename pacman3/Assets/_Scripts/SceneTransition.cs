using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class SceneTransition : MonoBehaviour
{
    public Image transition;
    private bool run;
    [SerializeField] private float transitionDuration = 1;
    private float startValue, endValue;
    private float elapsedTime;

    private void OnEnable() {
        NextScene.OnNextScene += TriggerOutro;
    }

    private void OnDisable() {
        NextScene.OnNextScene -= TriggerOutro;
    }

    private void Start() {
        startValue = 1;
        endValue = 0;
        run = true;
        transition.gameObject.SetActive(true);
    }

    private void Update() {
        if (run) {
            float complete = Fade(startValue, endValue);
            if (complete == 1) {
                run = false;
                if (endValue == 0) transition.gameObject.SetActive(false);
            }
        }
    }

    private float Fade(float start, float end) {
        elapsedTime += Time.deltaTime;
        float percentageComplete = elapsedTime / transitionDuration;
        if (elapsedTime > transitionDuration) {
            percentageComplete = 1;
        }

        float alpha = Mathf.Lerp(startValue, endValue, percentageComplete);
        transition.color = new Color(transition.color.r, transition.color.g, transition.color.b, alpha);

        return percentageComplete;
    }

    private void TriggerOutro() {
        startValue = 0;
        endValue = 1;
        run = true;
        elapsedTime = 0;
        transition.gameObject.SetActive(true);
    }
}
