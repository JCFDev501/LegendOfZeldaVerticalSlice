using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeManager : MonoBehaviour
{
    private List<Transform> m_nodes;
    [SerializeField] private float m_radius = 5f; // Define the radius within which to find nodes
    private float moveSpeed; // Define the speed of movement
    private PlayerController m_playerController = null;

    void Start()
    {
        m_playerController = GetComponent<PlayerController>();

        moveSpeed = m_playerController.GetMovmentSpeed();

        // Find all game objects with the tag "Node" and add their transforms to the nodes list
        m_nodes = new List<Transform>();
        GameObject[] nodeObjects = GameObject.FindGameObjectsWithTag("Node");
        foreach (GameObject nodeObject in nodeObjects)
        {
            m_nodes.Add(nodeObject.transform);
        }
    }

    public void MoveToNextNodeInDirection(int dir)
    {
        // Move to the closest node in the specified direction (1: North, 2: South, 3: East, 4: West) within a given m_radius
        Transform closestNode = null;
        float closestDistance = float.MaxValue;

        foreach (Transform node in m_nodes)
        {
            Vector3 directionToNode = node.position - transform.position;
            float distanceToNode = directionToNode.magnitude;

            if (distanceToNode <= m_radius && IsInDirection(dir, directionToNode) && distanceToNode < closestDistance)
            {
                closestNode = node;
                closestDistance = distanceToNode;
            }
        }

        if (closestNode != null)
        {
            StartCoroutine(SmoothMove(transform.position, closestNode.position));
        }
    }

    public void MoveBackOverNodes(int dir, int numberOfNodes)
    {
        // Move back over a certain number of nodes in the specified direction
        List<Transform> nodesInDirection = new List<Transform>();

        foreach (Transform node in m_nodes)
        {
            Vector3 directionToNode = node.position - transform.position;

            if (IsInDirection(dir, directionToNode))
            {
                nodesInDirection.Add(node);
            }
        }

        nodesInDirection.Sort((a, b) => (b.position - transform.position).magnitude.CompareTo((a.position - transform.position).magnitude));

        StartCoroutine(MoveBackOverNodesSmoothly(nodesInDirection, numberOfNodes));
    }

    private IEnumerator MoveBackOverNodesSmoothly(List<Transform> nodesInDirection, int numberOfNodes)
    {
        for (int i = 0; i < numberOfNodes && i < nodesInDirection.Count; i++)
        {
            Vector3 startPosition = transform.position;
            Vector3 endPosition = nodesInDirection[i].position;
            float elapsedTime = 0;

            while (elapsedTime < moveSpeed)
            {
                transform.position = Vector3.Lerp(startPosition, endPosition, (elapsedTime / moveSpeed));
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            transform.position = endPosition;
        }
    }

    private IEnumerator SmoothMove(Vector3 start, Vector3 end)
    {
        float elapsedTime = 0;
        while (elapsedTime < moveSpeed)
        {
            transform.position = Vector3.Lerp(start, end, (elapsedTime / moveSpeed));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.position = end;
    }

    private bool IsInDirection(int dir, Vector3 direction)
    {
        switch (dir)
        {
            case 1: return direction.y > 0 && Mathf.Abs(direction.x) < Mathf.Abs(direction.y); // North
            case 2: return direction.y < 0 && Mathf.Abs(direction.x) < Mathf.Abs(direction.y); // South
            case 3: return direction.x > 0 && Mathf.Abs(direction.y) < Mathf.Abs(direction.x); // East
            case 4: return direction.x < 0 && Mathf.Abs(direction.y) < Mathf.Abs(direction.x); // West
            default: return false;
        }
    }
}
