using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddMeshColider : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform childObject in transform)
        {
            // First we get the Mesh attached to the child object
            Mesh mesh = childObject.gameObject.GetComponent<MeshFilter>().mesh;

            // If we've found a mesh we can use it to add a collider
            if (mesh != null)
            {
                // Add a new MeshCollider to the child object
                MeshCollider meshCollider = childObject.gameObject.AddComponent<MeshCollider>();

                // Finaly we set the Mesh in the MeshCollider
                meshCollider.sharedMesh = mesh;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
