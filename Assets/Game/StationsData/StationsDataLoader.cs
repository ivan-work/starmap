using System;
using System.Collections.Generic;
using System.IO;
using Game.Station_Status;
using Game.StationsData;
using Game.Utils;
using Library;
using UnityEngine;
using UnityEngine.Networking;

namespace Game {
  public class StationsDataLoader : PlainSingleton<StationsDataLoader> {
    private const string FileName = "stations.csv";

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
      var baseUri = uri.GetLeftPart(UriPartial.Authority);
      Debug.Log($"Path is {baseUri}/{FileName}");
      var www = UnityWebRequest.Get($"{baseUri}/{FileName}");
      await www.SendWebRequest();
      if (www.result == UnityWebRequest.Result.Success) {
        Debug.Log($"result is {www.downloadHandler.text}");
        return www.downloadHandler.text;
      }

      Debug.Log($"Error is {www.error}");

      return "";
    }

    public async Awaitable<StationsDataFile> Load(List<StationStatus> stationStatusList) {
      var result = new StationsDataFile();
      var fileData = "";
      Debug.Log( Application.absoluteURL);
#if !UNITY_EDITOR && UNITY_WEBGL
      Debug.Log($"Loading file from web");
      fileData = await loadFileWeb(FileName);
#else
      Debug.Log($"Loading file from system");
      fileData = await loadFileSystem(FileName);
#endif
      var index = 0;

      var csv = CSVReader.Read(fileData);
      foreach (var stationDataCsv in csv) {
        var statusIndexString = stationDataCsv.GetValueOrDefault("status", "");
        var name = stationDataCsv.GetValueOrDefault("name", "");
        var description = stationDataCsv.GetValueOrDefault("description", "");
        var stationData = new StationData() {
          Name = name ?? $"Station #{index}", // ...
          Description = description ?? "",
          Status = stationStatusList[SafeIntParse(statusIndexString)]
        };
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
