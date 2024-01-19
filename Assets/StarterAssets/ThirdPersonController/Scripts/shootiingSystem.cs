using System;
using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class shootingSystem : MonoBehaviour
{
    [SerializeField] private GameObject _shootingTarget;
    [SerializeField] private GameObject _shootingOrigin;
    [SerializeField] private GameObject impact;
    [SerializeField] private ParticleSystem particules;
    [SerializeField] private float _maxShootDistance = 10000f;
    [SerializeField] private float _fireRate = 0.5f;
    [SerializeField] private bool _isReloading;
    private StarterAssetsInputs _input;

    private Camera _mainCamera;
    public TextMeshProUGUI ammo;
    private Counter _counter;
    private const int MaxAmmo = 30;
    private int _magAmmo = 30;
    private float _time;
    private float _nextTimeTofire;
    private Animator _animator;


    // Start is called before the first frame update
    void Start()
    {
        _input = GetComponent<StarterAssetsInputs>();
        _mainCamera = Camera.main;
        _animator = GetComponent<Animator>();
        _counter = GetComponentInParent<Counter>();
    }
    
    // Update is called once per frame
    private void Update()
    {
        Debug.Log(_magAmmo);
        if (!_isReloading)
        {
            _time += Time.deltaTime;
            _nextTimeTofire = 1 / _fireRate;
            if (_time >= _nextTimeTofire && _input.isShooting && _input.isAiming)
            {
                if (_magAmmo<=0)
                {
                    StartCoroutine(reload());
                }
                else
                {
                    Shoot();
                    _time = 0;
                }
            }
            if(_input.isReloading)
            {
                _isReloading = true;
            }
        }
        else
        {
            StartCoroutine(reload());
        }
        _input.isReloading = false;
        ammo.text = _magAmmo + "/" + MaxAmmo;
        var par = particules.main;
        par.loop ^= true;
    }

    void Shoot()
    {
        _shootingTarget.SetActive(false); 
        _shootingOrigin.SetActive(false); 
        Vector3 shootPoint = _mainCamera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, _maxShootDistance));
        Ray ray = new Ray(_shootingOrigin.transform.position,shootPoint -_shootingOrigin.transform.position);
        Debug.DrawRay(ray.origin,ray.direction * _maxShootDistance,Color.green, 0.5f);
        if (Physics.Raycast(ray, out RaycastHit hitInfo,_maxShootDistance)) ;
        {
            _shootingTarget.transform.position = hitInfo.point;
            if (hitInfo.collider.gameObject.CompareTag("target"))
            {
                _counter.AddPoints();
            }
            if (!hitInfo.collider.gameObject.CompareTag("Player"))
            {
                Instantiate(impact, hitInfo.point, Quaternion.identity);
                StartCoroutine(particule());
            }
            particules.Play();
            _magAmmo--;
        }
    }

    IEnumerator reload()
    {
        _animator.SetBool("reload", true);
        yield return new WaitForSeconds(2);
        _animator.SetBool("reload", false);
        _magAmmo = MaxAmmo;
        _isReloading = false;
    }
    
    IEnumerator particule()
    {
        particules.loop=true;
        yield return new WaitForSeconds(0.05f);
        particules.loop = false;

    }
    
}
    