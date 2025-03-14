using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextScene : MonoBehaviour
{
    public delegate void SceneEvent();
    public static event SceneEvent OnNextScene;

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Return)) {
            StartCoroutine(DelayNextScene());
            if (OnNextScene != null) {
                OnNextScene();
            }
        }
    }

    private IEnumerator DelayNextScene() {
        yield return new WaitForSeconds(1.2f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
