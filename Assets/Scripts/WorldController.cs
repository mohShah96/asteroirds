﻿using System.Collections.Generic;
using Moshah.Asteroids.Models;
using UnityEngine;
using Zenject;

namespace Moshah.Asteroids.Gameplay
{
    public class WorldController : MonoBehaviour, IWorldController
    {
        [HideInInspector] public float minX;
        [HideInInspector] public float maxX;
        [HideInInspector] public float minY;
        [HideInInspector] public float maxY;

        private readonly List<IFloatingObject> _floatingObjects = new List<IFloatingObject>();
        [Inject] private GameConfig _gameConfig;

        private void Start()
        {
            var mainCam = Camera.main;
            var ratio = (float) Screen.width / Screen.height;
            minX = -mainCam.orthographicSize * ratio;
            maxX = mainCam.orthographicSize * ratio;
            minY = -mainCam.orthographicSize;
            maxY = mainCam.orthographicSize;
        }

        public void WrapFloatingObjectsPositionsIfOutsideBoundaries()
        {
            foreach (var floatingObject in _floatingObjects)
            {
                if (floatingObject.Position.x < minX - _gameConfig.boundaryThreshold)
                {
                    floatingObject.Position =
                        new Vector2(maxX + _gameConfig.boundaryThreshold, floatingObject.Position.y);
                }

                if (floatingObject.Position.x > maxX + _gameConfig.boundaryThreshold)
                {
                    floatingObject.Position =
                        new Vector2(minX - _gameConfig.boundaryThreshold, floatingObject.Position.y);
                }

                if (floatingObject.Position.y < minY - _gameConfig.boundaryThreshold)
                {
                    floatingObject.Position =
                        new Vector2(floatingObject.Position.x, maxY + _gameConfig.boundaryThreshold);
                }

                if (floatingObject.Position.y > maxY + _gameConfig.boundaryThreshold)
                {
                    floatingObject.Position =
                        new Vector2(floatingObject.Position.x, minY - _gameConfig.boundaryThreshold);
                }
            }
        }

        public void RegisterFloatingObject(IFloatingObject floatingObject)
        {
            _floatingObjects.Add(floatingObject);
        }

        public void RemoveFloatingObject(IFloatingObject floatingObject)
        {
            _floatingObjects.Remove(floatingObject);
        }

        private void Update()
        {
            WrapFloatingObjectsPositionsIfOutsideBoundaries();
        }
    }
}