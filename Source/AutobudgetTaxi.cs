﻿using ColossalFramework;
using ColossalFramework.IO;
using UnityEngine;

namespace AutoBudget
{
    public class AutobudgetTaxi : AutobudgetVehicles
    {
        public class Data : IDataContainer
        {
            public void Serialize(DataSerializer s)
            {
                AutobudgetTaxi d = Singleton<AutobudgetManager>.instance.container.AutobudgetTaxi;
                s.WriteBool(d.Enabled);
                s.WriteInt32(d.BudgetMaxValue);
                s.WriteInt32(d.TargetNumberOfVehiclesWaitingAtDepot);
                s.WriteInt32(d.TargetNumberOfVehiclesWaitingAtStand);
            }

            public void Deserialize(DataSerializer s)
            {
                AutobudgetTaxi d = Singleton<AutobudgetManager>.instance.container.AutobudgetTaxi;
                d.Enabled = s.ReadBool();
                d.BudgetMaxValue = s.ReadInt32();
                d.TargetNumberOfVehiclesWaitingAtDepot = s.ReadInt32();
                d.TargetNumberOfVehiclesWaitingAtStand = s.ReadInt32();
            }

            public void AfterDeserialize(DataSerializer s)
            {
                Debug.Log(Mod.LogMsgPrefix + "AutobudgetTaxi data loaded.");
            }
        }

        public int BudgetMaxValue = 110;
        public int TargetNumberOfVehiclesWaitingAtDepot = 1;
        public int TargetNumberOfVehiclesWaitingAtStand = 3;

        public override string GetEconomyPanelContainerName()
        {
            return "SubServicesBudgetContainer";
        }

        public override string GetBudgetItemName()
        {
            return "Taxi";
        }

        public override ItemClass.Service GetService()
        {
            return ItemClass.Service.PublicTransport;
        }

        public override ItemClass.SubService GetSubService()
        {
            return ItemClass.SubService.PublicTransportTaxi;
        }

        protected override int refreshCount
        {
            get
            {
                return oneDayFrames / 2 + 19;
            }
        }

        protected override void setAutobudget()
        {
            int taxiDepotCount = 0;
            int taxiStandCount = 0;

            foreach (ushort n in ServiceBuildingNs(ItemClass.Service.PublicTransport))
            {
                Building bld = Singleton<BuildingManager>.instance.m_buildings.m_buffer[(int)n];
                if ((bld.m_flags & Building.Flags.Active) == 0) continue;

                if (bld.Info.m_buildingAI is TaxiStandAI)
                {
                    taxiStandCount++;
                }
                else if (bld.Info.m_buildingAI is DepotAI)
                {
                    taxiDepotCount++;
                }
            }

            if (taxiDepotCount == 0) return;

            int buffer = TargetNumberOfVehiclesWaitingAtDepot + taxiStandCount * TargetNumberOfVehiclesWaitingAtStand / taxiDepotCount;

            setBudgetForVehicles(
                typeof(DepotAI),
                buffer,
                50,
                BudgetMaxValue);
        }
    }
}
