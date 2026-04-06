using System;
using System.Collections.Generic;
using System.IO;
using Game.Station_Status;
using Game.Utils;
using Library;
using UnityEngine;
using UnityEngine.Networking;

namespace Game.StationsData {
  public class StationsDataLoader : PlainSingleton<StationsDataLoader> {
    private const string LocalFileName = "stations.csv";

    private async Awaitable<string> loadFileSystem(string fileName) {
      var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "Resources", fileName);
      if (File.Exists(filePath)) {
        try {
          return File.ReadAllText(filePath);
        } catch (IOException ex) {
          Debug.LogError("Error reading file: " + ex.Message);
        }
      } else {
        Debug.LogError($"File not found at path: {filePath}");
      }

      return "";
    }

    private async Awaitable<string> loadFileWeb(string filepath) {
      Uri.TryCreate(Application.absoluteURL, UriKind.Absolute, out var uri);
      var baseUri = uri.GetLeftPart(UriPartial.Path);
      var path = $"{baseUri}{LocalFileName}";
      Debug.Log($"Path is {path}");
      var www = UnityWebRequest.Get($"{path}");
      await www.SendWebRequest();
      if (www.result == UnityWebRequest.Result.Success) {
        return www.downloadHandler.text;
      }

      Debug.Log($"Error is {www.error}");

      return "";
    }

    public async Awaitable<StationsDataFile> Load(List<StationStatus> stationStatusList) {
      var result = new StationsDataFile();
      var fileData = "";
#if !UNITY_EDITOR && UNITY_WEBGL
      Debug.Log($"Loading file from web");
      fileData = await loadFileWeb(LocalFileName);
#else
      Debug.Log($"Loading file from system");
      fileData = await loadFileSystem(LocalFileName);
#endif
      var index = 0;

      var csv = CSVReader.Read(fileData);
      Debug.Log($"Stations found: {csv.Count}");
      foreach (var stationCsv in csv) {
        var (statusIndexString, name, description, _) = stationCsv.ToArray();
        var stationData = new StationData() {
          Name = name ?? $"Station #{index}", // ...
          Description = description ?? "",
          Status = stationStatusList[SafeIntParse(statusIndexString)]
        };
        // Debug.Log($"Creating station {stationData.Name} + [{statusIndexString}/{SafeIntParse(statusIndexString)}]{stationData.Status.Status}");
        result.Stations.Add(index, stationData);
        index++;
      }

      return result;
    }

    private static int SafeIntParse(string str, int defaultResult = 0) {
      return int.TryParse(str, out var result) ? result : defaultResult;
    }
  }
}
