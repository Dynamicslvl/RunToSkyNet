using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;

public class StoryUI : MonoBehaviour
{
    [SerializeField]
    private Button startButton;

    [SerializeField]
    private GameObject story1, story2, character;

    public async void OnStartStory()
    {
        AudioManager.Instance.Play(AudioEnum.Tap);
        AudioManager.Instance.Play(AudioEnum.Home);
        story1.SetActive(true);
        character.SetActive(false);
        startButton.gameObject.SetActive(false);
        await UniTask.WaitForSeconds(7.5f);
        AudioManager.Instance.Play(AudioEnum.Crash);
        story1.SetActive(false);
        story2.SetActive(true);
        await UniTask.WaitForSeconds(2.5f);
        SceneLoader.Instance.LoadSceneWithoutLoadingCutScreen(1);
    }
}
