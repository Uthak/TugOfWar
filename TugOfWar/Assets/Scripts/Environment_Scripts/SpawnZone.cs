using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnZone : MonoBehaviour
{
    [Header("Spawn Zone Setup:")]
    public bool player1Zone = false;
    public bool player2Zone = false;

    [Space(10)]
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
    public void InitializeSpawnZone(int _zoneID)
    {
        switch(_zoneID)
        {
            case 0:
                // DO NOT DELETE:
                //_meshRenderer.material = _neutralColorMat; //enable if you want to see neutral zones:
                return;

            case 1:
                _meshRenderer.material = _team1ColorMat;
                player1Zone = true;
                return;

            case 2:
                // DO NOT DELETE:
                //_meshRenderer.material = _team2ColorMat; //enable if you want to see player 2 deployment zones:
                player2Zone = true;
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
