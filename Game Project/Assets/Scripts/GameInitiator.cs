using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class GameInitiator : MonoBehaviour
{
    [SerializeField] private GameObject _mainMenuController;
    [SerializeField] private IAPManager _iapManager;
    [SerializeField] private AdsManager _adsManager;
    [SerializeField] private GameObject _audioManager;
    [SerializeField] private GameObject _mainCamera;

    private async void Start()
    {
        Bindings();
        await InitializeServices();
        await InitializeAudio();
        await InitializeUI();
    }

    private async Task InitializeAudio()
    {
        await Task.Yield();
        _audioManager.SetActive(true);
        _audioManager.GetComponent<AudioManager>().EnableSound();
    }

    private async Task InitializeUI()
    {
        await Task.Yield();

        var menu = _mainMenuController.GetComponent<MainMenuController>();
        menu.InitiateFirst();   
        menu.InitiateSecond();
        menu.InitiateThird();
    }

    private async Task InitializeServices()
    {
        await _adsManager.InitializeAds();

        await _iapManager.Initiate();
    }

    private void Bindings()
    {
        _mainMenuController = Instantiate(_mainMenuController);
        _iapManager = Instantiate(_iapManager);
        _adsManager = Instantiate(_adsManager);
        _mainCamera = Instantiate(_mainCamera);
        _audioManager = Instantiate(_audioManager);
    }
}
