using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnZone : MonoBehaviour
{
    public bool occupied = false;

    [SerializeField] Material _neutralColorMat;
    [SerializeField] Material _team1ColorMat;
    [SerializeField] Material _team2ColorMat;
    [SerializeField] MeshRenderer _meshRenderer;

    /// <summary>
    /// When creating SpawnZones this function is called to allocate the correct material of the zone. 
    /// 0 = Neutral, 1 = Team 1, 2 = Team 2.
    /// </summary>
    /// <param name="_zoneID"></param>
    public void SetupSpawnZone(int _zoneID)
    {
        switch(_zoneID)
        {
            case 0:
                _meshRenderer.material = _neutralColorMat;
                return;

            case 1:
                _meshRenderer.material = _team1ColorMat;
                return;

            case 2:
                _meshRenderer.material = _team2ColorMat;
                return;
        }
    }
    public void OccupyDeploymentTile()
    {
        occupied = true;
        _meshRenderer.enabled = false;
    }
    public void VacateDeploymentTile()
    {
        occupied = false;
        _meshRenderer.enabled = true;
    }
}
