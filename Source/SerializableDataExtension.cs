﻿using System;
using System.IO;
using ColossalFramework.IO;
using ICities;
using UnityEngine;

namespace Autobudget
{
    public class SerializableDataExtension : ISerializableDataExtension
    {
        public const string DataID = "ComprehensiveAutoBudgetMod";
        public const uint DataVersion = 6;
        public static uint LastReadDataVersion = 0;
        private ISerializableData serializedData;

        public void OnCreated(ISerializableData serializedData)
        {
            this.serializedData = serializedData;
        }

        public void OnReleased()
        {
            serializedData = null;
        }

        public void OnLoadData()
        {
            try
            {
                byte[] data = serializedData.LoadData(DataID);

                if (data == null)
                {
                    Debug.Log(Mod.LogMsgPrefix + "No saved data ");
                    return;
                }

                using (var stream = new MemoryStream(data))
                {
                    DataSerializer.Deserialize<AutobudgetElectricity.Data>(stream, DataSerializer.Mode.Memory);
                    DataSerializer.Deserialize<AutobudgetWater.Data>(stream, DataSerializer.Mode.Memory);
                    if (LastReadDataVersion >= 6) DataSerializer.Deserialize<AutobudgetHeating.Data>(stream, DataSerializer.Mode.Memory);
                    DataSerializer.Deserialize<AutobudgetGarbage.Data>(stream, DataSerializer.Mode.Memory);
                    DataSerializer.Deserialize<AutobudgetHealthcare.Data>(stream, DataSerializer.Mode.Memory);
                    DataSerializer.Deserialize<AutobudgetEducation.Data>(stream, DataSerializer.Mode.Memory);
                    DataSerializer.Deserialize<AutobudgetPolice.Data>(stream, DataSerializer.Mode.Memory);
                    if (LastReadDataVersion >= 5) DataSerializer.Deserialize<AutobudgetIndustry.Data>(stream, DataSerializer.Mode.Memory);
                    DataSerializer.Deserialize<AutobudgetFire.Data>(stream, DataSerializer.Mode.Memory);
                    DataSerializer.Deserialize<AutobudgetRoad.Data>(stream, DataSerializer.Mode.Memory);
                    if (LastReadDataVersion >= 4) DataSerializer.Deserialize<AutobudgetPost.Data>(stream, DataSerializer.Mode.Memory);
                    DataSerializer.Deserialize<AutobudgetTaxi.Data>(stream, DataSerializer.Mode.Memory);
                }
            }
            catch (Exception ex)
            {
                Debug.Log(Mod.LogMsgPrefix + "load error: " + ex.Message);
            }

            Mod.UpdateUI();
            BudgetControlsManager.UpdateControls();
        }

        public void OnSaveData()
        {
            try
            {
                byte[] data;

                using (var stream = new MemoryStream())
                {
                    DataSerializer.Serialize(stream, DataSerializer.Mode.Memory, DataVersion, new AutobudgetElectricity.Data());
                    DataSerializer.Serialize(stream, DataSerializer.Mode.Memory, DataVersion, new AutobudgetWater.Data());
                    DataSerializer.Serialize(stream, DataSerializer.Mode.Memory, DataVersion, new AutobudgetHeating.Data());
                    DataSerializer.Serialize(stream, DataSerializer.Mode.Memory, DataVersion, new AutobudgetGarbage.Data());
                    DataSerializer.Serialize(stream, DataSerializer.Mode.Memory, DataVersion, new AutobudgetHealthcare.Data());
                    DataSerializer.Serialize(stream, DataSerializer.Mode.Memory, DataVersion, new AutobudgetEducation.Data());
                    DataSerializer.Serialize(stream, DataSerializer.Mode.Memory, DataVersion, new AutobudgetPolice.Data());
                    DataSerializer.Serialize(stream, DataSerializer.Mode.Memory, DataVersion, new AutobudgetIndustry.Data());
                    DataSerializer.Serialize(stream, DataSerializer.Mode.Memory, DataVersion, new AutobudgetFire.Data());
                    DataSerializer.Serialize(stream, DataSerializer.Mode.Memory, DataVersion, new AutobudgetRoad.Data());
                    DataSerializer.Serialize(stream, DataSerializer.Mode.Memory, DataVersion, new AutobudgetPost.Data());
                    DataSerializer.Serialize(stream, DataSerializer.Mode.Memory, DataVersion, new AutobudgetTaxi.Data());
                    data = stream.ToArray();
                }

                serializedData.SaveData(DataID, data);
            }
            catch (Exception ex)
            {
                Debug.Log(Mod.LogMsgPrefix + "save error: " + ex.Message);
            }
        }
    }
}
