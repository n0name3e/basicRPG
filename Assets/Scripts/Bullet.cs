﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Vector3 direction;
    private float speed;
    private IDamageable sender;

    public void Launch(Vector3 _direction, float _speed, IDamageable _sender)
    {
        direction = _direction;
        speed = _speed;
        sender = _sender;
        transform.Rotate(0, 0, Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg - 90);
        //transform.Rotate(0, 0, Mathf.Asin(_direction.y) * Mathf.Rad2Deg - 90);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        IDamageable _damageable = collision.gameObject.GetComponent<IDamageable>();
        if (_damageable != null && _damageable != sender)
        {
            _damageable.Hit(20, sender);
        }
    }
    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
        //transform.transform.Translate(speed * Time.deltaTime * direction);
    }
}