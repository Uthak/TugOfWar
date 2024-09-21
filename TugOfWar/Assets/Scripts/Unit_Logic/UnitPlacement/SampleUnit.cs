using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnitEnumManager;

public class SampleUnit
{
    private GameObject _sampleModel;
    private GameObject _unitPrefab;

    private Material _validPlacementMaterial;    // Custom material for valid placement (green)
    private Material _invalidPlacementMaterial;  // Custom material for invalid placement (red)
    private Material _defaultMaterial;           // Custom default material for neutral state

    private List<Renderer> _meshRenderers;


    public SampleUnit(GameObject unitPrefab, Material defaultMaterial, Material validMaterial, Material invalidMaterial)
    {
        _unitPrefab = unitPrefab;
        _defaultMaterial = defaultMaterial;
        _validPlacementMaterial = validMaterial;
        _invalidPlacementMaterial = invalidMaterial;

        // Retrieve the sample model from the pool
        _sampleModel = UnitPoolManager.Instance.GetUnitFromPool(_unitPrefab);
        _sampleModel.SetActive(false);

        // Get all mesh renderers of the sample model
        _meshRenderers = new List<Renderer>(_sampleModel.GetComponentsInChildren<Renderer>());
    }

    // Show the sample model
    public void EnableSampleModel()
    {
        _sampleModel.SetActive(true);
        SetSampleMaterial(SampleUnitMaterial.Default); // Start with the default material
    }

    // Update the sample model position based on cursor location
    public void UpdateSampleModelPosition(Vector3 newPosition)
    {
        if (_sampleModel != null)
        {
            _sampleModel.transform.position = newPosition;
        }
    }

    // Change the material based on the placement state
    public void SetSampleMaterial(SampleUnitMaterial state)
    {
        Material chosenMaterial = _defaultMaterial; // Default to the neutral material

        switch (state)
        {
            case SampleUnitMaterial.Valid:
                chosenMaterial = _validPlacementMaterial;
                break;
            case SampleUnitMaterial.Invalid:
                chosenMaterial = _invalidPlacementMaterial;
                break;
        }

        // Apply the material to all mesh renderers
        foreach (Renderer renderer in _meshRenderers)
        {
            renderer.material = chosenMaterial;
        }
    }

    // Disable the sample model and return it to the pool
    public void DisableSampleModel()
    {
        if (_sampleModel != null)
        {
            UnitPoolManager.Instance.ReturnUnitToPool(_unitPrefab, _sampleModel);
            _sampleModel = null;
        }
    }
}
