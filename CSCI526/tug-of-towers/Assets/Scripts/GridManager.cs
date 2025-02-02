using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] private int _width, _height;    // Grid width and height

    [SerializeField] private Plot _plotPrefab;       // The prefab reference is now of type Plot

    [SerializeField] private Transform _cam;         // Camera reference for centering

    private Dictionary<Vector2, Plot> _plots;        // Dictionary to hold generated plots

    void Start()
    {
        GenerateGrid();   // Generate the grid when the game starts
    }

    void GenerateGrid()
    {
        // Initialize the dictionary to store plots
        _plots = new Dictionary<Vector2, Plot>();

        // Loop through the grid size
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                // Instantiate a Plot at each grid position
                var spawnedPlot = Instantiate(_plotPrefab, new Vector3(x, y), Quaternion.identity);
                spawnedPlot.name = $"Plot {x} {y}";  // Name the plot based on its position

                // Calculate an offset pattern (alternating colors or other visual effects)
                var isOffset = (x % 2 == 0 && y % 2 != 0) || (x % 2 != 0 && y % 2 == 0);

                // Add the plot to the dictionary with its position as the key
                _plots[new Vector2(x, y)] = spawnedPlot;
            }
        }

        // Center the camera over the grid
        _cam.transform.position = new Vector3((float)_width / 2 - 0.5f, (float)_height / 2 - 0.5f, -10);
    }

    // Method to get a Plot at a specific grid position
    public Plot GetPlotAtPosition(Vector2 pos)
    {
        if (_plots.TryGetValue(pos, out var plot)) return plot;
        return null;  // Return null if the position is out of the grid's bounds
    }
}
