using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChangeAgentAreaMask : MonoBehaviour
{
    private const string _agentTag = "Agent";
    private const string _agentName = "AGENT";
    private bool _originalArea = true;

    private const string _areaStreet = "Street";

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Space");
            GameObject[] agents = GameObject.FindGameObjectsWithTag(_agentTag);
            foreach (GameObject agent in agents)
            {
                if (agent.name != _agentName) { continue; }

                NavMeshAgent nma = agent.GetComponent<NavMeshAgent>();
                if (nma == null) { continue; }
                int areaMask = nma.areaMask;

                if (_originalArea)
                {
                    areaMask += 1 << NavMesh.GetAreaFromName(_areaStreet);
                }
                else
                {
                    areaMask -= 1 << NavMesh.GetAreaFromName(_areaStreet);
                }
                nma.areaMask = areaMask;
                nma.ResetPath();
            }
            _originalArea = !_originalArea;
        }
    }
}