using System.Collections.Generic;
using Core.Interface;
using UnityEngine;

namespace Core
{
    public class Presenters : IPresenter
    {
        private List<IPreInitialisation> _preInitializePresenters;
        private List<IInitialisation> _initializePresenters;
        private List<IExecute> _executePresenters;
        private List<IFixedExecute> _fixedExecutePresenters;
        private List<ICleanUp> _cleanupPresenters;
        
        internal Presenters()
        {
            Init();
        }

        private void Init()
        {
            _preInitializePresenters = new List<IPreInitialisation>();
            _initializePresenters = new List<IInitialisation>();
            _executePresenters = new List<IExecute>();
            _fixedExecutePresenters = new List<IFixedExecute>();
            _cleanupPresenters = new List<ICleanUp>();
        }

        public Presenters Add(IPresenter presenter)
        {
            if (presenter is IPreInitialisation)
            {
                _preInitializePresenters.Add((IPreInitialisation)presenter);
            }
            if (presenter is IInitialisation)
            {
                _initializePresenters.Add((IInitialisation)presenter);
            }
            if (presenter is IExecute)
            {
                _executePresenters.Add((IExecute)presenter);
            }
            if (presenter is IFixedExecute)
            {
                _fixedExecutePresenters.Add((IFixedExecute)presenter);
            }
            if (presenter is ICleanUp)
            {
                _cleanupPresenters.Add((ICleanUp)presenter);
            }
            return this;
        }

        public void PreInitialization()
        {
            for (var i = 0; i < _preInitializePresenters.Count; ++i)
            {
                _preInitializePresenters[i].PreInitialisation();
            }
        }

        public void Initialization()
        {
            for (var i = 0; i < _initializePresenters.Count; ++i)
            {
                _initializePresenters[i].Initialisation();
            }
        }

        public void Execute(float deltaTime)
        {
            for (var index = 0; index < _executePresenters.Count; ++index)
            {
                _executePresenters[index].Execute(deltaTime);
            }
        }

        public void FixedExecute(float fixedDeltaTime)
        {
            for (var index = 0; index < _fixedExecutePresenters.Count; ++index)
            {
                _fixedExecutePresenters[index].FixedExecute(fixedDeltaTime);
            }
        }

        public void Cleanup()
        {
            for (var index = 0; index < _cleanupPresenters.Count; ++index)
            {
                _cleanupPresenters[index].Cleanup();
            }
        }
    }
}
