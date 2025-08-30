//-----------------------------\\
//              Project HITHC
//    Author: Joshua Hughes
//        Twitch.tv/neokuro
//-----------------------------\\

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Hzn.Framework;

using UnityEngine;

public class FileReader : MonoBehaviour
{
    public struct TrophyNameData
    {
        public string identifier;
        public string displayName;
        public string[] winnerNames;

        public TrophyNameData(string id, string display, string[] winners)
        {
            identifier = id;
            displayName = display;
            winnerNames = winners;
        }

        public static TrophyNameData FromCSV(string linesOfCSV)
        {
            try
            {
                string[] values = linesOfCSV.Split(',');
                string id = values[0];
                string display = values[1];
                string[] names = values.Skip(2).ToArray();
                return new TrophyNameData(id, display, names);
            }
            catch (Exception e)
            {
                Dbg.Error(Log.Tools,$"Failed to convert CSV file to TrophyNameData. Exception: {e.Message}\nStackTrace: {e.StackTrace}");
            }

            return new TrophyNameData("-999", "-999", null);
        }
    }

  //  public static List<SubData> LoadSubDataFromCSV(string urlToFile)
  //  {
  //      List<SubData> subData = new List<SubData>();
  //      try
  //      {
  //          subData = File.ReadAllLines(urlToFile)
  //                                  .Skip(1)
  //                                  .Select(v => SubData.FromCSV(v))
  //                                  .ToList();
  //      }
  //      catch (Exception e)
  //      {
  //          // Delete the file path if it couldn't be found
  //          PlayerPrefs.DeleteKey(Secrets.SUBLIST_FILE_PATH);
  //          DebugManager.Warn("Failed to load file {0}", urlToFile);
  //      }

  //      if (subData.Count > 0)
  //      {
  //          PlayerPrefs.SetString(Secrets.SUBLIST_FILE_PATH, urlToFile);
  //          PlayerPrefs.Save();
  //      }
		////SavedCharacters.CSVUpdate(subData);
		//return subData;
  //  }

  //  public static List<PointsData> LoadPointsDataFromCSV(string urlToFile)
  //  {
  //      bool success = true;
  //      List<PointsData> pData = new List<PointsData>();
  //      try
  //      {
  //          pData = File.ReadAllLines(urlToFile)
  //                                  .Skip(1)
  //                                  .Select(v => PointsData.FromCSV(v))
  //                                  .ToList();
  //      }
  //      catch(Exception e)
  //      {
  //          PlayerPrefs.DeleteKey(Secrets.POINTSDATA_FILE_PATH);
  //          DebugManager.Warn("Failed to load file {0}", urlToFile);
  //          success = false;
  //      }

  //      if(success)
  //      {
  //          PlayerPrefs.SetString(Secrets.POINTSDATA_FILE_PATH, urlToFile);
  //          PlayerPrefs.Save();
  //      }

  //      return pData;
  //  }

  //  public static List<TrophyNameData> LoadTrophyNamedataFromCSV(string urlToFile)
  //  {
  //      List<TrophyNameData> nameData = new List<TrophyNameData>();
  //      try
  //      {
  //          nameData = File.ReadAllLines(urlToFile)
  //                          .Skip(1)
  //                          .Select(v => TrophyNameData.FromCSV(v))
  //                          .ToList();
  //      }
  //      catch(Exception e)
  //      {
  //          PlayerPrefs.DeleteKey(Secrets.TROPHYNAMEIST_FILE_PATH);
  //          DebugManager.Warn("Failed to load file {0}", urlToFile);
  //      }

  //      if(nameData.Count > 0)
  //      {
  //          PlayerPrefs.SetString(Secrets.TROPHYNAMEIST_FILE_PATH, urlToFile);
  //          PlayerPrefs.Save();
  //      }

  //      return nameData;
  //  }

    public static void SaveDataToCSV(string urlToFile, string[] headers, StringBuilder perRowData)
    {
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < headers.Length; i++)
        {
            sb.Append(headers[i]);
            if(i < headers.Length - 1)
            {
                sb.Append(",");
            }
        }

        sb.AppendLine();
        sb.Append(perRowData);

        using(StreamWriter sw = new StreamWriter(urlToFile, false))
        {
            sw.Write(sb.ToString());
        }
    }
}