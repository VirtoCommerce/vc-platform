using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace VirtoCommerce.Client.Model
{
    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public partial class VirtoCommerceOrderModuleWebModelDashboardStatisticsResult :  IEquatable<VirtoCommerceOrderModuleWebModelDashboardStatisticsResult>
    {
        /// <summary>
        /// Gets or Sets StartDate
        /// </summary>
        [DataMember(Name="startDate", EmitDefaultValue=false)]
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// Gets or Sets EndDate
        /// </summary>
        [DataMember(Name="endDate", EmitDefaultValue=false)]
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// Gets or Sets Revenue
        /// </summary>
        [DataMember(Name="revenue", EmitDefaultValue=false)]
        public List<VirtoCommerceOrderModuleWebModelMoney> Revenue { get; set; }

        /// <summary>
        /// Gets or Sets RevenuePeriodDetails
        /// </summary>
        [DataMember(Name="revenuePeriodDetails", EmitDefaultValue=false)]
        public List<VirtoCommerceOrderModuleWebModelQuarterPeriodMoney> RevenuePeriodDetails { get; set; }

        /// <summary>
        /// Gets or Sets OrderCount
        /// </summary>
        [DataMember(Name="orderCount", EmitDefaultValue=false)]
        public int? OrderCount { get; set; }

        /// <summary>
        /// Gets or Sets CustomersCount
        /// </summary>
        [DataMember(Name="customersCount", EmitDefaultValue=false)]
        public int? CustomersCount { get; set; }

        /// <summary>
        /// Gets or Sets RevenuePerCustomer
        /// </summary>
        [DataMember(Name="revenuePerCustomer", EmitDefaultValue=false)]
        public List<VirtoCommerceOrderModuleWebModelMoney> RevenuePerCustomer { get; set; }

        /// <summary>
        /// Gets or Sets AvgOrderValue
        /// </summary>
        [DataMember(Name="avgOrderValue", EmitDefaultValue=false)]
        public List<VirtoCommerceOrderModuleWebModelMoney> AvgOrderValue { get; set; }

        /// <summary>
        /// Gets or Sets AvgOrderValuePeriodDetails
        /// </summary>
        [DataMember(Name="avgOrderValuePeriodDetails", EmitDefaultValue=false)]
        public List<VirtoCommerceOrderModuleWebModelQuarterPeriodMoney> AvgOrderValuePeriodDetails { get; set; }

        /// <summary>
        /// Gets or Sets ItemsPurchased
        /// </summary>
        [DataMember(Name="itemsPurchased", EmitDefaultValue=false)]
        public int? ItemsPurchased { get; set; }

        /// <summary>
        /// Gets or Sets LineitemsPerOrder
        /// </summary>
        [DataMember(Name="lineitemsPerOrder", EmitDefaultValue=false)]
        public double? LineitemsPerOrder { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class VirtoCommerceOrderModuleWebModelDashboardStatisticsResult {\n");
            sb.Append("  StartDate: ").Append(StartDate).Append("\n");
            sb.Append("  EndDate: ").Append(EndDate).Append("\n");
            sb.Append("  Revenue: ").Append(Revenue).Append("\n");
            sb.Append("  RevenuePeriodDetails: ").Append(RevenuePeriodDetails).Append("\n");
            sb.Append("  OrderCount: ").Append(OrderCount).Append("\n");
            sb.Append("  CustomersCount: ").Append(CustomersCount).Append("\n");
            sb.Append("  RevenuePerCustomer: ").Append(RevenuePerCustomer).Append("\n");
            sb.Append("  AvgOrderValue: ").Append(AvgOrderValue).Append("\n");
            sb.Append("  AvgOrderValuePeriodDetails: ").Append(AvgOrderValuePeriodDetails).Append("\n");
            sb.Append("  ItemsPurchased: ").Append(ItemsPurchased).Append("\n");
            sb.Append("  LineitemsPerOrder: ").Append(LineitemsPerOrder).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }
  
        /// <summary>
        /// Returns the JSON string presentation of the object
        /// </summary>
        /// <returns>JSON string presentation of the object</returns>
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }

        /// <summary>
        /// Returns true if objects are equal
        /// </summary>
        /// <param name="obj">Object to be compared</param>
        /// <returns>Boolean</returns>
        public override bool Equals(object obj)
        {
            // credit: http://stackoverflow.com/a/10454552/677735
            return this.Equals(obj as VirtoCommerceOrderModuleWebModelDashboardStatisticsResult);
        }

        /// <summary>
        /// Returns true if VirtoCommerceOrderModuleWebModelDashboardStatisticsResult instances are equal
        /// </summary>
        /// <param name="other">Instance of VirtoCommerceOrderModuleWebModelDashboardStatisticsResult to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(VirtoCommerceOrderModuleWebModelDashboardStatisticsResult other)
        {
            // credit: http://stackoverflow.com/a/10454552/677735
            if (other == null)
                return false;

            return 
                (
                    this.StartDate == other.StartDate ||
                    this.StartDate != null &&
                    this.StartDate.Equals(other.StartDate)
                ) && 
                (
                    this.EndDate == other.EndDate ||
                    this.EndDate != null &&
                    this.EndDate.Equals(other.EndDate)
                ) && 
                (
                    this.Revenue == other.Revenue ||
                    this.Revenue != null &&
                    this.Revenue.SequenceEqual(other.Revenue)
                ) && 
                (
                    this.RevenuePeriodDetails == other.RevenuePeriodDetails ||
                    this.RevenuePeriodDetails != null &&
                    this.RevenuePeriodDetails.SequenceEqual(other.RevenuePeriodDetails)
                ) && 
                (
                    this.OrderCount == other.OrderCount ||
                    this.OrderCount != null &&
                    this.OrderCount.Equals(other.OrderCount)
                ) && 
                (
                    this.CustomersCount == other.CustomersCount ||
                    this.CustomersCount != null &&
                    this.CustomersCount.Equals(other.CustomersCount)
                ) && 
                (
                    this.RevenuePerCustomer == other.RevenuePerCustomer ||
                    this.RevenuePerCustomer != null &&
                    this.RevenuePerCustomer.SequenceEqual(other.RevenuePerCustomer)
                ) && 
                (
                    this.AvgOrderValue == other.AvgOrderValue ||
                    this.AvgOrderValue != null &&
                    this.AvgOrderValue.SequenceEqual(other.AvgOrderValue)
                ) && 
                (
                    this.AvgOrderValuePeriodDetails == other.AvgOrderValuePeriodDetails ||
                    this.AvgOrderValuePeriodDetails != null &&
                    this.AvgOrderValuePeriodDetails.SequenceEqual(other.AvgOrderValuePeriodDetails)
                ) && 
                (
                    this.ItemsPurchased == other.ItemsPurchased ||
                    this.ItemsPurchased != null &&
                    this.ItemsPurchased.Equals(other.ItemsPurchased)
                ) && 
                (
                    this.LineitemsPerOrder == other.LineitemsPerOrder ||
                    this.LineitemsPerOrder != null &&
                    this.LineitemsPerOrder.Equals(other.LineitemsPerOrder)
                );
        }

        /// <summary>
        /// Gets the hash code
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode()
        {
            // credit: http://stackoverflow.com/a/263416/677735
            unchecked // Overflow is fine, just wrap
            {
                int hash = 41;
                // Suitable nullity checks etc, of course :)

                if (this.StartDate != null)
                    hash = hash * 59 + this.StartDate.GetHashCode();

                if (this.EndDate != null)
                    hash = hash * 59 + this.EndDate.GetHashCode();

                if (this.Revenue != null)
                    hash = hash * 59 + this.Revenue.GetHashCode();

                if (this.RevenuePeriodDetails != null)
                    hash = hash * 59 + this.RevenuePeriodDetails.GetHashCode();

                if (this.OrderCount != null)
                    hash = hash * 59 + this.OrderCount.GetHashCode();

                if (this.CustomersCount != null)
                    hash = hash * 59 + this.CustomersCount.GetHashCode();

                if (this.RevenuePerCustomer != null)
                    hash = hash * 59 + this.RevenuePerCustomer.GetHashCode();

                if (this.AvgOrderValue != null)
                    hash = hash * 59 + this.AvgOrderValue.GetHashCode();

                if (this.AvgOrderValuePeriodDetails != null)
                    hash = hash * 59 + this.AvgOrderValuePeriodDetails.GetHashCode();

                if (this.ItemsPurchased != null)
                    hash = hash * 59 + this.ItemsPurchased.GetHashCode();

                if (this.LineitemsPerOrder != null)
                    hash = hash * 59 + this.LineitemsPerOrder.GetHashCode();

                return hash;
            }
        }

    }
}
