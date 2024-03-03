using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Provides menu entries to quickly add folders of Terrain layers/trees/detail meshes/detail textures to
/// a Terrain component.
/// 
/// Usage:
///   1. Select a game object with a Terrain component from the Hierarchy. (Otherwise menu
///   entries will be greyed out.)
///   2. Open submenu `Tools > Terrain Folder Tools` and select appropriate prototype to import.
///   3. Choose folder with layers/prefabs/textures of the given type.
/// 
/// Credits:
///   Base implementation provided by llnk7 on the Unity Forums:
///   https://forum.unity.com/threads/easily-add-many-trees-to-your-terrain.225679/#post-1751723
///   Updated and expanded by https://github.com/hheimbuerger/
/// </summary>
public class TerrainFolderTools {
    [MenuItem("Tools/Terrain Folder Tools/Populate Terrain Layers from folder", priority = 101)]
    static void PopulateLayersFromFolder() {
        TerrainData currentTerrainData = GetSelectedTerrainData();
        List<TerrainLayer> layers = new List<TerrainLayer>(currentTerrainData.terrainLayers);
        foreach (TerrainLayer layer in GetGameObjectsFromUserChosenFolder<TerrainLayer>(
                     "Select the folder containing Tree prefabs", "Assets/")) {
            layers.Add(layer);
        }

        currentTerrainData.terrainLayers = layers.ToArray();
        ReloadTerrain();
    }

    [MenuItem("Tools/Terrain Folder Tools/Clear all Terrain Layers", priority = 102)]
    static void ClearLayerEditor() {
        GetSelectedTerrainData().terrainLayers = null;
        ReloadTerrain();
    }

    [MenuItem("Tools/Terrain Folder Tools/Populate Tree prototypes from folder", priority = 201)]
    static void PopulateTreesFromFolder() {
        TerrainData currentTerrainData = GetSelectedTerrainData();
        List<TreePrototype> treePrototypesList = new List<TreePrototype>(currentTerrainData.treePrototypes);
        foreach (GameObject prefab in GetGameObjectsFromUserChosenFolder<GameObject>(
                     "Select the folder containing Tree prefabs", "Assets/")) {
            treePrototypesList.Add(new TreePrototype() { prefab = prefab });
        }

        currentTerrainData.treePrototypes = treePrototypesList.ToArray();
        ReloadTerrain();
    }

    [MenuItem("Tools/Terrain Folder Tools/Clear all Tree prototypes", priority = 202)]
    static void ClearTreeEditor() {
        GetSelectedTerrainData().treePrototypes = null;
        ReloadTerrain();
    }

    [MenuItem("Tools/Terrain Folder Tools/Populate Detail mesh prototypes from folder", priority = 301)]
    static void PopulateDetailMeshesFromFolder() {
        TerrainData currentTerrainData = GetSelectedTerrainData();
        List<DetailPrototype> detailPrototypes = new List<DetailPrototype>(currentTerrainData.detailPrototypes);
        foreach (GameObject prefab in GetGameObjectsFromUserChosenFolder<GameObject>(
                     "Select the folder containing Detail mesh prototypes", "Assets/")) {
            detailPrototypes.Add(new DetailPrototype() { prototype = prefab, usePrototypeMesh = true });
        }

        currentTerrainData.detailPrototypes = detailPrototypes.ToArray();
        ReloadTerrain();
    }

    [MenuItem("Tools/Terrain Folder Tools/Populate Detail texture prototypes from folder", priority = 302)]
    static void PopulateDetailTexturesFromFolder() {
        TerrainData currentTerrainData = GetSelectedTerrainData();
        List<DetailPrototype> detailPrototypes = new List<DetailPrototype>(currentTerrainData.detailPrototypes);
        foreach (Texture2D texture in GetGameObjectsFromUserChosenFolder<Texture2D>(
                     "Select the folder containing Detail texture prototypes", "Assets/")) {
            detailPrototypes.Add(new DetailPrototype() { prototypeTexture = texture });
        }

        currentTerrainData.detailPrototypes = detailPrototypes.ToArray();
        ReloadTerrain();
    }

    [MenuItem("Tools/Terrain Folder Tools/Clear all Detail prototypes", priority = 303)]
    static void ClearDetailsEditor() {
        GetSelectedTerrainData().detailPrototypes = null;
        ReloadTerrain();
    }

    [MenuItem("Tools/Terrain Folder Tools/Populate Terrain Layers from folder", isValidateFunction: true)]
    [MenuItem("Tools/Terrain Folder Tools/Clear all Terrain Layers", isValidateFunction: true)]
    [MenuItem("Tools/Terrain Folder Tools/Populate Tree prototypes from folder", isValidateFunction: true)]
    [MenuItem("Tools/Terrain Folder Tools/Clear all Tree prototypes", isValidateFunction: true)]
    [MenuItem("Tools/Terrain Folder Tools/Populate Detail mesh prototypes from folder", isValidateFunction: true)]
    [MenuItem("Tools/Terrain Folder Tools/Populate Detail texture prototypes from folder", isValidateFunction: true)]
    [MenuItem("Tools/Terrain Folder Tools/Clear all Detail prototypes", isValidateFunction: true)]
    static bool ValidateHasTerrainSelected() {
        return CheckTerrainSelected();
    }

    private static bool CheckTerrainSelected() {
        if (Selection.activeGameObject == null || GetSelectedTerrain() == null) {
            // Debug.LogWarning("You must have a Terrain selected to perform this action!");
            return false;
        }

        return true;
    }

    private static Terrain GetSelectedTerrain() {
        return Selection.activeGameObject.GetComponent<Terrain>();
    }

    private static TerrainData GetSelectedTerrainData() {
        return GetSelectedTerrain().terrainData;
    }

    private static void ReloadTerrain() {
        Terrain terrain = GetSelectedTerrain();
        terrain.Flush();
        terrain.terrainData.RefreshPrototypes();
        EditorUtility.SetDirty(terrain);
    }

    private static IEnumerable<T> GetGameObjectsFromUserChosenFolder<T>(string userMessage,
        string startPath = "Assets/") where T : UnityEngine.Object {
        string folder = EditorUtility.OpenFolderPanel(userMessage, startPath, "");
        if (folder != "") {
            if (folder.IndexOf(Application.dataPath) == -1) {
                Debug.LogWarning("The folder must be in this project anywhere inside the Assets folder!");
                throw new Exception("The folder must be in this project anywhere inside the Assets folder!");
            }

            string[] files = Directory.GetFiles(folder);
            if (files.Length > 0) {
                for (int i = 0; i < files.Length; i++) {
                    string relativePath = files[i].Substring(files[i].IndexOf("Assets/"));
                    T gameObject = AssetDatabase.LoadAssetAtPath<T>(relativePath);
                    if (gameObject != null)
                        yield return gameObject;
                }
            }
        }
    }
}
