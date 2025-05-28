using System;
using Visualizer.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Visualizer.Visualization;

namespace Visualizer.IO
{
    public class DropdownHelper : MonoBehaviour
    {
        private TMP_Dropdown _dropdown;
        private AppManager _appManager;
        private bool _isUpdatingProgrammatically = false;


        private void Awake()
        {
            _dropdown = GetComponent<TMP_Dropdown>();

            if (!_dropdown)
            {
                Debug.LogError("DropdownHelper: No Dropdown component found.");
            }

            _appManager = FindAnyObjectByType<AppManager>();

            if (!_appManager)
            {
                Debug.LogError("DropdownHelper: No AppManager found in the scene.");
            }
            else
            {
                _appManager.OnShadingModeChanged += HandleExternalShadingModeChange;
            }

            PopulateDropdownFromEnum();
        }

        private void OnDestroy()
        {
            if (_appManager != null)
            {
                _appManager.OnShadingModeChanged -= HandleExternalShadingModeChange;
            }
        }

        private void PopulateDropdownFromEnum()
        {
            _dropdown.ClearOptions();
            var options = new System.Collections.Generic.List<string>(Enum.GetNames(typeof(ShadingMode)));
            _dropdown.AddOptions(options);
        }

        public void OnDropdownValueChanged()
        {

            if (_isUpdatingProgrammatically) return; // Prevent loopback from the event

            if (!_dropdown || !_appManager) return;

            int selectedIndex = _dropdown.value;

            if (System.Enum.IsDefined(typeof(ShadingMode), selectedIndex))
            {
                ShadingMode selectedMode = (ShadingMode)selectedIndex;
                _appManager.SetShadingMode(selectedMode);
            }
            else
            {
                Debug.LogWarning("DropdownHelper: Selected index does not correspond to a valid ShadingMode enum.");
            }
        }

        private void HandleExternalShadingModeChange(ShadingMode mode)
        {
            _isUpdatingProgrammatically = true;
            _dropdown.value = (int)mode;
            _dropdown.RefreshShownValue();
            _isUpdatingProgrammatically = false;
        }
    }
}