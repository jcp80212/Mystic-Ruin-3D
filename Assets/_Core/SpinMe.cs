﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RPG.Core
{
    public class SpinMe : MonoBehaviour
    {

        [SerializeField] float xRotationsPerMinute = 1f;
        [SerializeField] float yRotationsPerMinute = 1f;
        [SerializeField] float zRotationsPerMinute = 1f;

        void Update()
        {
            // xDegreesPerFrame = Time.DeltaTime, 60, 360, xRotationsPerMinute
            // degrees frame ^ -1 = seconds per frame ^ -1 / seconds minute ^ -1, degrees rotation^-1 * rotation minute^-1
            // degrees frame ^-1 = frame ^-1 minute, degrees rotation ^-1* rotation minute ^-1
            // degrees frame ^-1 = frame^-1*degree


            float xDegreesPerFrame = Time.deltaTime / 60 * 360 * xRotationsPerMinute; // TODO COMPLETE ME
            transform.RotateAround(transform.position, transform.right, xDegreesPerFrame);

            float yDegreesPerFrame = Time.deltaTime / 60 * 360 * yRotationsPerMinute; // TODO COMPLETE ME
            transform.RotateAround(transform.position, transform.up, yDegreesPerFrame);

            float zDegreesPerFrame = Time.deltaTime / 60 * 360 * zRotationsPerMinute; // TODO COMPLETE ME
            transform.RotateAround(transform.position, transform.forward, zDegreesPerFrame);
        }
    }
}
