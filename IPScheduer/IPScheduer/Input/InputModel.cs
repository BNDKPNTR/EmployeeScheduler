using System;
using System.Collections.Generic;
using System.Text;

namespace SchedulingIP.Input
{



    // NOTE: Generated code may require at least .NET Framework 4.5 or .NET Core/Standard 2.0.
    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class SchedulingPeriod
    {

        private System.DateTime startDateField;

        private System.DateTime endDateField;

        private SchedulingPeriodShift[] shiftTypesField;

        private SchedulingPeriodShiftGroups shiftGroupsField;

        private SchedulingPeriodEmployee[] employeesField;

        private SchedulingPeriodContracts contractsField;

        private object skillGroupsField;

        private SchedulingPeriodCoverRequirements coverRequirementsField;

        private SchedulingPeriodEmployee2[] fixedAssignmentsField;

        private object rulesField;

        private object employeePairingsField;

        private object dayOffRequestsField;

        private object dayOnRequestsField;

        private object shiftOffRequestsField;

        private object shiftOnRequestsField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
        public System.DateTime StartDate
        {
            get
            {
                return this.startDateField;
            }
            set
            {
                this.startDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
        public System.DateTime EndDate
        {
            get
            {
                return this.endDateField;
            }
            set
            {
                this.endDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Shift", IsNullable = false)]
        public SchedulingPeriodShift[] ShiftTypes
        {
            get
            {
                return this.shiftTypesField;
            }
            set
            {
                this.shiftTypesField = value;
            }
        }

        /// <remarks/>
        public SchedulingPeriodShiftGroups ShiftGroups
        {
            get
            {
                return this.shiftGroupsField;
            }
            set
            {
                this.shiftGroupsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Employee", IsNullable = false)]
        public SchedulingPeriodEmployee[] Employees
        {
            get
            {
                return this.employeesField;
            }
            set
            {
                this.employeesField = value;
            }
        }

        /// <remarks/>
        public SchedulingPeriodContracts Contracts
        {
            get
            {
                return this.contractsField;
            }
            set
            {
                this.contractsField = value;
            }
        }

        /// <remarks/>
        public object SkillGroups
        {
            get
            {
                return this.skillGroupsField;
            }
            set
            {
                this.skillGroupsField = value;
            }
        }

        /// <remarks/>
        public SchedulingPeriodCoverRequirements CoverRequirements
        {
            get
            {
                return this.coverRequirementsField;
            }
            set
            {
                this.coverRequirementsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Employee", IsNullable = false)]
        public SchedulingPeriodEmployee2[] FixedAssignments
        {
            get
            {
                return this.fixedAssignmentsField;
            }
            set
            {
                this.fixedAssignmentsField = value;
            }
        }

        /// <remarks/>
        public object Rules
        {
            get
            {
                return this.rulesField;
            }
            set
            {
                this.rulesField = value;
            }
        }

        /// <remarks/>
        public object EmployeePairings
        {
            get
            {
                return this.employeePairingsField;
            }
            set
            {
                this.employeePairingsField = value;
            }
        }

        /// <remarks/>
        public object DayOffRequests
        {
            get
            {
                return this.dayOffRequestsField;
            }
            set
            {
                this.dayOffRequestsField = value;
            }
        }

        /// <remarks/>
        public object DayOnRequests
        {
            get
            {
                return this.dayOnRequestsField;
            }
            set
            {
                this.dayOnRequestsField = value;
            }
        }

        /// <remarks/>
        public object ShiftOffRequests
        {
            get
            {
                return this.shiftOffRequestsField;
            }
            set
            {
                this.shiftOffRequestsField = value;
            }
        }

        /// <remarks/>
        public object ShiftOnRequests
        {
            get
            {
                return this.shiftOnRequestsField;
            }
            set
            {
                this.shiftOnRequestsField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class SchedulingPeriodShift
    {

        private string nameField;

        private string colorField;

        private byte timeUnitsField;

        private string startTimeField;

        private string endTimeField;

        private string idField;

        /// <remarks/>
        public string Name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        public string Color
        {
            get
            {
                return this.colorField;
            }
            set
            {
                this.colorField = value;
            }
        }

        /// <remarks/>
        public byte TimeUnits
        {
            get
            {
                return this.timeUnitsField;
            }
            set
            {
                this.timeUnitsField = value;
            }
        }

        /// <remarks/>
        public string StartTime
        {
            get
            {
                return this.startTimeField;
            }
            set
            {
                this.startTimeField = value;
            }
        }

        /// <remarks/>
        public string EndTime
        {
            get
            {
                return this.endTimeField;
            }
            set
            {
                this.endTimeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string ID
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class SchedulingPeriodShiftGroups
    {

        private SchedulingPeriodShiftGroupsShiftGroup shiftGroupField;

        /// <remarks/>
        public SchedulingPeriodShiftGroupsShiftGroup ShiftGroup
        {
            get
            {
                return this.shiftGroupField;
            }
            set
            {
                this.shiftGroupField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class SchedulingPeriodShiftGroupsShiftGroup
    {

        private string[] shiftField;

        private string idField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Shift")]
        public string[] Shift
        {
            get
            {
                return this.shiftField;
            }
            set
            {
                this.shiftField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string ID
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class SchedulingPeriodEmployee
    {

        private string contractIDField;

        private string[] skillsField;

        private string idField;

        /// <remarks/>
        public string ContractID
        {
            get
            {
                return this.contractIDField;
            }
            set
            {
                this.contractIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Skill", IsNullable = false)]
        public string[] Skills
        {
            get
            {
                return this.skillsField;
            }
            set
            {
                this.skillsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string ID
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class SchedulingPeriodContracts
    {

        private SchedulingPeriodContractsContract contractField;

        /// <remarks/>
        public SchedulingPeriodContractsContract Contract
        {
            get
            {
                return this.contractField;
            }
            set
            {
                this.contractField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class SchedulingPeriodContractsContract
    {

        private SchedulingPeriodContractsContractWorkload workloadField;

        private SchedulingPeriodContractsContractMinSeq[] minSeqField;

        private SchedulingPeriodContractsContractMaxSeq maxSeqField;

        private SchedulingPeriodContractsContractMinRestTime minRestTimeField;

        private string idField;

        /// <remarks/>
        public SchedulingPeriodContractsContractWorkload Workload
        {
            get
            {
                return this.workloadField;
            }
            set
            {
                this.workloadField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("MinSeq")]
        public SchedulingPeriodContractsContractMinSeq[] MinSeq
        {
            get
            {
                return this.minSeqField;
            }
            set
            {
                this.minSeqField = value;
            }
        }

        /// <remarks/>
        public SchedulingPeriodContractsContractMaxSeq MaxSeq
        {
            get
            {
                return this.maxSeqField;
            }
            set
            {
                this.maxSeqField = value;
            }
        }

        /// <remarks/>
        public SchedulingPeriodContractsContractMinRestTime MinRestTime
        {
            get
            {
                return this.minRestTimeField;
            }
            set
            {
                this.minRestTimeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string ID
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class SchedulingPeriodContractsContractWorkload
    {

        private SchedulingPeriodContractsContractWorkloadTimeUnits timeUnitsField;

        /// <remarks/>
        public SchedulingPeriodContractsContractWorkloadTimeUnits TimeUnits
        {
            get
            {
                return this.timeUnitsField;
            }
            set
            {
                this.timeUnitsField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class SchedulingPeriodContractsContractWorkloadTimeUnits
    {

        private SchedulingPeriodContractsContractWorkloadTimeUnitsMax maxField;

        /// <remarks/>
        public SchedulingPeriodContractsContractWorkloadTimeUnitsMax Max
        {
            get
            {
                return this.maxField;
            }
            set
            {
                this.maxField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class SchedulingPeriodContractsContractWorkloadTimeUnitsMax
    {

        private byte countField;

        private SchedulingPeriodContractsContractWorkloadTimeUnitsMaxWeight weightField;

        private string labelField;

        /// <remarks/>
        public byte Count
        {
            get
            {
                return this.countField;
            }
            set
            {
                this.countField = value;
            }
        }

        /// <remarks/>
        public SchedulingPeriodContractsContractWorkloadTimeUnitsMaxWeight Weight
        {
            get
            {
                return this.weightField;
            }
            set
            {
                this.weightField = value;
            }
        }

        /// <remarks/>
        public string Label
        {
            get
            {
                return this.labelField;
            }
            set
            {
                this.labelField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class SchedulingPeriodContractsContractWorkloadTimeUnitsMaxWeight
    {

        private string functionField;

        private byte valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string function
        {
            get
            {
                return this.functionField;
            }
            set
            {
                this.functionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public byte Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class SchedulingPeriodContractsContractMinSeq
    {

        private string labelField;

        private byte valueField;

        private string shiftField;

        private ushort weightField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string label
        {
            get
            {
                return this.labelField;
            }
            set
            {
                this.labelField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string shift
        {
            get
            {
                return this.shiftField;
            }
            set
            {
                this.shiftField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public ushort weight
        {
            get
            {
                return this.weightField;
            }
            set
            {
                this.weightField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class SchedulingPeriodContractsContractMaxSeq
    {

        private string labelField;

        private byte valueField;

        private string shiftField;

        private ushort weightField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string label
        {
            get
            {
                return this.labelField;
            }
            set
            {
                this.labelField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string shift
        {
            get
            {
                return this.shiftField;
            }
            set
            {
                this.shiftField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public ushort weight
        {
            get
            {
                return this.weightField;
            }
            set
            {
                this.weightField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class SchedulingPeriodContractsContractMinRestTime
    {

        private string labelField;

        private ushort weightField;

        private ushort valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string label
        {
            get
            {
                return this.labelField;
            }
            set
            {
                this.labelField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public ushort weight
        {
            get
            {
                return this.weightField;
            }
            set
            {
                this.weightField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public ushort Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class SchedulingPeriodCoverRequirements
    {

        private SchedulingPeriodCoverRequirementsDayOfWeekCover dayOfWeekCoverField;

        /// <remarks/>
        public SchedulingPeriodCoverRequirementsDayOfWeekCover DayOfWeekCover
        {
            get
            {
                return this.dayOfWeekCoverField;
            }
            set
            {
                this.dayOfWeekCoverField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class SchedulingPeriodCoverRequirementsDayOfWeekCover
    {

        private string dayField;

        private SchedulingPeriodCoverRequirementsDayOfWeekCoverCover[] coverField;

        /// <remarks/>
        public string Day
        {
            get
            {
                return this.dayField;
            }
            set
            {
                this.dayField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Cover")]
        public SchedulingPeriodCoverRequirementsDayOfWeekCoverCover[] Cover
        {
            get
            {
                return this.coverField;
            }
            set
            {
                this.coverField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class SchedulingPeriodCoverRequirementsDayOfWeekCoverCover
    {

        private string shiftField;

        private SchedulingPeriodCoverRequirementsDayOfWeekCoverCoverMin minField;

        private SchedulingPeriodCoverRequirementsDayOfWeekCoverCoverMax maxField;

        /// <remarks/>
        public string Shift
        {
            get
            {
                return this.shiftField;
            }
            set
            {
                this.shiftField = value;
            }
        }

        /// <remarks/>
        public SchedulingPeriodCoverRequirementsDayOfWeekCoverCoverMin Min
        {
            get
            {
                return this.minField;
            }
            set
            {
                this.minField = value;
            }
        }

        /// <remarks/>
        public SchedulingPeriodCoverRequirementsDayOfWeekCoverCoverMax Max
        {
            get
            {
                return this.maxField;
            }
            set
            {
                this.maxField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class SchedulingPeriodCoverRequirementsDayOfWeekCoverCoverMin
    {

        private ushort weightField;

        private byte valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public ushort weight
        {
            get
            {
                return this.weightField;
            }
            set
            {
                this.weightField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public byte Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class SchedulingPeriodCoverRequirementsDayOfWeekCoverCoverMax
    {

        private ushort weightField;

        private byte valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public ushort weight
        {
            get
            {
                return this.weightField;
            }
            set
            {
                this.weightField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public byte Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class SchedulingPeriodEmployee2
    {

        private string employeeIDField;

        private SchedulingPeriodEmployeeAssign[] assignField;

        /// <remarks/>
        public string EmployeeID
        {
            get
            {
                return this.employeeIDField;
            }
            set
            {
                this.employeeIDField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Assign")]
        public SchedulingPeriodEmployeeAssign[] Assign
        {
            get
            {
                return this.assignField;
            }
            set
            {
                this.assignField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class SchedulingPeriodEmployeeAssign
    {

        private string shiftField;

        private System.DateTime dateField;

        /// <remarks/>
        public string Shift
        {
            get
            {
                return this.shiftField;
            }
            set
            {
                this.shiftField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
        public System.DateTime Date
        {
            get
            {
                return this.dateField;
            }
            set
            {
                this.dateField = value;
            }
        }
    }

}