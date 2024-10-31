using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class Navigation : MonoBehaviour
{
    [SerializeField] Transform Player;
    [SerializeField] List<GameObject> Destinations;
    [SerializeField] Material Material;
    [SerializeField] TMP_Dropdown Dropdown;
    private NavMeshPath path;
    private float elapsed = 0.0f;
    private Vector3 currentDestionation;
    private LineRenderer lineRenderer;
    private float lineWidth = 0.1f;
    // Start is called before the first frame update
    void Start()
    {
        List<TMP_Dropdown.OptionData> options = new List<TMP_Dropdown.OptionData>();
        TMP_Dropdown.OptionData option = new TMP_Dropdown.OptionData("Not selected");
        options.Add(option);

        foreach (var value in Destinations)
        {
            option = new TMP_Dropdown.OptionData(value.name);
            options.Add(option);
        }

        // Set the TMP Dropdown options.
        Dropdown.ClearOptions(); // Clear any existing options.
        Dropdown.AddOptions(options); // Add the new options.

        path = new NavMeshPath();
        elapsed = 0.0f;

        //draw line
        lineRenderer = this.gameObject.AddComponent<LineRenderer>();
        lineRenderer.material = Material;
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;
    }

    // Update is called once per frame
    void Update()
    {
        // Update the way to the goal every second.
        UpdateCurrentPath();
        //draw path
        DrawPathWithLineRender();
    }

    public void DropDownDestionationSwitch()
    {
        var value = Dropdown.value;
        if (value == 0)
        {
            currentDestionation = Vector3.zero;
        }
        else if (value >= 1)
        {
            currentDestionation = Destinations[value-1].transform.position;
            Debug.Log("DropDown value changed!");
        }
    }

    private void UpdateCurrentPath()
    {
        elapsed += Time.deltaTime;
        if (elapsed > 1.0f)
        {
            elapsed -= 1.0f;
            if (currentDestionation != Vector3.zero)
            {
                NavMesh.CalculatePath(Player.transform.position, currentDestionation, NavMesh.AllAreas, path);

                if (path.corners.Length == 0)
                {
                    Debug.Log("Path not found to " + Destinations[Dropdown.value - 1].name + ". " +
                        "Maybe the destination object is not close to navigation mesh or no continous path can " +
                        "be found to the destination.");
                }
            }
        }
    }

    private void DrawPathWithLineRender()
    {
        //draw path via lineRender
        if (currentDestionation != Vector3.zero)
        {
            lineRenderer.positionCount = path.corners.Length;
            for (int i = 0; i < path.corners.Length; i++)
            {
                lineRenderer.SetPosition(i, path.corners[i]);
            }
        }
        else
        {
            lineRenderer.positionCount = 0;
        }
    }
}
