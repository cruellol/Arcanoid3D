using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace Arcanoid
{
    public enum Complicacy { Easy, Medium, Hard }
    public class SettingValues
    {
        public bool VolumeOff;
        public float Volume;
        public Complicacy Complication;

        public SettingValues(bool volumeOff, float volume, Complicacy compli)
        {
            VolumeOff = volumeOff;
            Volume = volume;
            Complication = compli;
        }
    }
    public class SettingSaver : MonoBehaviour
    {
        private string _settingPath = "SavedSettings.txt";
        [SerializeField]
        private Toggle _volumeToggle;
        [SerializeField]
        private Slider _volumeSlider;
        [SerializeField]
        private Toggle _easyToggle;
        [SerializeField]
        private Toggle _mediumToggle;
        [SerializeField]
        private Toggle _hardToggle;

        private void Start()
        {
            _volumeSlider.onValueChanged.AddListener(delegate { SettingsChanged(); });
            _volumeToggle.onValueChanged.AddListener(delegate { SettingsChanged(); });
            _easyToggle.onValueChanged.AddListener(delegate { SettingsChanged(); });
            _mediumToggle.onValueChanged.AddListener(delegate { SettingsChanged(); });
            _hardToggle.onValueChanged.AddListener(delegate { SettingsChanged(); });
        }

        private void SettingsChanged()
        {
            Complicacy currentComplicacy = Complicacy.Easy;
            if (_easyToggle.isOn)
            {

            }
            if (_mediumToggle.isOn)
            {
                currentComplicacy = Complicacy.Medium;
            }
            if (_hardToggle.isOn)
            {
                currentComplicacy = Complicacy.Hard;
            }
            SettingValues currentsettings = new SettingValues(_volumeToggle.isOn, _volumeSlider.value, currentComplicacy);
            using (var write = new StreamWriter(_settingPath))
            {
                write.WriteLine(JsonUtility.ToJson(currentsettings));
            }
        }

        private void OnEnable()
        {
            if (File.Exists(_settingPath))
            {
                using (var read = new StreamReader(_settingPath))
                {
                    SettingValues savedvalues = (SettingValues)JsonUtility.FromJson(read.ReadToEnd(), typeof(SettingValues));
                    SetSettings(savedvalues);
                }
            }
            else
            {
                SetSettings(new SettingValues(true,0.5f,Complicacy.Medium));
            }
        }

        private void SetSettings(SettingValues savedvalues)
        {
            _volumeToggle.isOn = savedvalues.VolumeOff;
            _volumeSlider.value = savedvalues.Volume;
            switch (savedvalues.Complication)
            {
                case Complicacy.Hard:
                    {
                        _hardToggle.isOn = true;
                        break;
                    }
                case Complicacy.Medium:
                    {
                        _mediumToggle.isOn = true;
                        break;
                    }
                case Complicacy.Easy:
                default:
                    {
                        _easyToggle.isOn = true;
                        break;
                    }
            }
        }

        private void OnDisable()
        {

        }
    }

}