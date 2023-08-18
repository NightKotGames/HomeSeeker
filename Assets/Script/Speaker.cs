using Spine.Unity;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animator), typeof(AudioSource))]

public class Speaker : MonoBehaviour
{
    [Header("Alarm Speaker MaxVolume: ")]
    [SerializeField, Range(.1f, 1f)] private float _maxVolume;
    [SerializeField, Range(.1f, 5f)] private float _fadeOutDuration = 2.0f; // Длительность затухания звука

    private Animator _animator;
    private SkeletonAnimation _animation;
    private AudioSource _audioSource;
    private Coroutine _fadeOutCoroutine; // Ссылка на текущую корутину затухания

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _animation = _animator.GetComponent<SkeletonAnimation>();
        _audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        AlarmZoneDetector.AlarmTriggered += OnSetAlarm;
    }

    private void OnDisable()
    {
        AlarmZoneDetector.AlarmTriggered -= OnSetAlarm;
    }

    private void OnSetAlarm(bool _alarmState, GameObject _enemy)
    {
        if (_alarmState)
        {
            TurnOnAlarm();
        }
        else
        {
            // Остановить предыдущую корутину, если она существует
            if (_fadeOutCoroutine != null)
            {
                StopCoroutine(_fadeOutCoroutine);
            }            
            _fadeOutCoroutine = StartCoroutine(FadeOutAndTurnOff());
        }
    }
    private void TurnOnAlarm()
    {
        _animator.SetBool("Alarm", true);
        _animation.loop = true;
        _animation.Initialize(true);
        _audioSource.Play();
        _fadeOutCoroutine = null; // Обнулить ссылку на корутину, чтобы остановить затухание
        StartCoroutine(SetVolume(_audioSource, _maxVolume));
    }

    private IEnumerator FadeOutAndTurnOff()
    {
        float startVolume = _audioSource.volume;
        float elapsedTime = 0.0f;

        while (elapsedTime < _fadeOutDuration)
        {
            float t = elapsedTime / _fadeOutDuration;
            _audioSource.volume = Mathf.Lerp(startVolume, 0f, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        _audioSource.Stop();
        _audioSource.volume = startVolume; // Восстановить начальную громкость
        _fadeOutCoroutine = null; // Очистить ссылку на корутину

        // Переключить анимацию на "покой"
        _animator.SetBool("Alarm", false);
    }


    private IEnumerator SetVolume(AudioSource _poleSpeakerSource, float _targetVolume)
    {
        float _currentVolume = _poleSpeakerSource.volume;
        float elapsedTime = 3f;

        while (Mathf.Abs(_currentVolume - _targetVolume) > 0.01f)
        {
            elapsedTime += Time.deltaTime;

            if (elapsedTime >= 0.2f)
            {
                _currentVolume = Mathf.MoveTowards(_currentVolume, _targetVolume, _maxVolume / 10);
                _poleSpeakerSource.volume = _currentVolume;
                elapsedTime = 0f;
            }
            yield return null;
        }
    }
}