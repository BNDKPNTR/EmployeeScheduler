namespace IPScheduler.Inputs
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

        private SchedulingPeriodShiftTypes shiftTypesField;

        private SchedulingPeriodContract[] contractsField;

        private SchedulingPeriodEmployee[] employeesField;

        private SchedulingPeriodEmployee1[] fixedAssignmentsField;

        private SchedulingPeriodShiftOff[] shiftOffRequestsField;

        private SchedulingPeriodShiftOn[] shiftOnRequestsField;

        private SchedulingPeriodDateSpecificCover[] coverRequirementsField;

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
        public SchedulingPeriodShiftTypes ShiftTypes
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
        [System.Xml.Serialization.XmlArrayItemAttribute("Contract", IsNullable = false)]
        public SchedulingPeriodContract[] Contracts
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
        [System.Xml.Serialization.XmlArrayItemAttribute("Employee", IsNullable = false)]
        public SchedulingPeriodEmployee1[] FixedAssignments
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
        [System.Xml.Serialization.XmlArrayItemAttribute("ShiftOff", IsNullable = false)]
        public SchedulingPeriodShiftOff[] ShiftOffRequests
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
        [System.Xml.Serialization.XmlArrayItemAttribute("ShiftOn", IsNullable = false)]
        public SchedulingPeriodShiftOn[] ShiftOnRequests
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

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("DateSpecificCover", IsNullable = false)]
        public SchedulingPeriodDateSpecificCover[] CoverRequirements
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
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class SchedulingPeriodShiftTypes
    {

        private SchedulingPeriodShiftTypesShift shiftField;

        /// <remarks/>
        public SchedulingPeriodShiftTypesShift Shift
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
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class SchedulingPeriodShiftTypesShift
    {

        private string colorField;

        private string startTimeField;

        private ushort durationField;

        private string idField;

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
        public ushort Duration
        {
            get
            {
                return this.durationField;
            }
            set
            {
                this.durationField = value;
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
    public partial class SchedulingPeriodContract
    {

        private SchedulingPeriodContractMaxSeq maxSeqField;

        private SchedulingPeriodContractMinSeq[] minSeqField;

        private SchedulingPeriodContractTimeUnits[] workloadField;

        private SchedulingPeriodContractPatterns patternsField;

        private SchedulingPeriodContractValidShifts validShiftsField;

        private SchedulingPeriodContractMinRestTime minRestTimeField;

        private string idField;

        /// <remarks/>
        public SchedulingPeriodContractMaxSeq MaxSeq
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
        [System.Xml.Serialization.XmlElementAttribute("MinSeq")]
        public SchedulingPeriodContractMinSeq[] MinSeq
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
        [System.Xml.Serialization.XmlArrayItemAttribute("TimeUnits", IsNullable = false)]
        public SchedulingPeriodContractTimeUnits[] Workload
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
        public SchedulingPeriodContractPatterns Patterns
        {
            get
            {
                return this.patternsField;
            }
            set
            {
                this.patternsField = value;
            }
        }

        /// <remarks/>
        public SchedulingPeriodContractValidShifts ValidShifts
        {
            get
            {
                return this.validShiftsField;
            }
            set
            {
                this.validShiftsField = value;
            }
        }

        /// <remarks/>
        public SchedulingPeriodContractMinRestTime MinRestTime
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
    public partial class SchedulingPeriodContractMaxSeq
    {

        private string labelField;

        private byte valueField;

        private string shiftField;

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
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class SchedulingPeriodContractMinSeq
    {

        private string labelField;

        private byte valueField;

        private string shiftField;

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
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class SchedulingPeriodContractTimeUnits
    {

        private SchedulingPeriodContractTimeUnitsMin minField;

        private SchedulingPeriodContractTimeUnitsMax maxField;

        /// <remarks/>
        public SchedulingPeriodContractTimeUnitsMin Min
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
        public SchedulingPeriodContractTimeUnitsMax Max
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
    public partial class SchedulingPeriodContractTimeUnitsMin
    {

        private ushort countField;

        private string labelField;

        /// <remarks/>
        public ushort Count
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
    public partial class SchedulingPeriodContractTimeUnitsMax
    {

        private ushort countField;

        private string labelField;

        /// <remarks/>
        public ushort Count
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
    public partial class SchedulingPeriodContractPatterns
    {

        private SchedulingPeriodContractPatternsMatch matchField;

        /// <remarks/>
        public SchedulingPeriodContractPatternsMatch Match
        {
            get
            {
                return this.matchField;
            }
            set
            {
                this.matchField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class SchedulingPeriodContractPatternsMatch
    {

        private SchedulingPeriodContractPatternsMatchMax maxField;

        private SchedulingPeriodContractPatternsMatchPattern[] patternField;

        /// <remarks/>
        public SchedulingPeriodContractPatternsMatchMax Max
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

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("Pattern")]
        public SchedulingPeriodContractPatternsMatchPattern[] Pattern
        {
            get
            {
                return this.patternField;
            }
            set
            {
                this.patternField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class SchedulingPeriodContractPatternsMatchMax
    {

        private byte countField;

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
    public partial class SchedulingPeriodContractPatternsMatchPattern
    {

        private string startDayField;

        private string[] shiftField;

        /// <remarks/>
        public string StartDay
        {
            get
            {
                return this.startDayField;
            }
            set
            {
                this.startDayField = value;
            }
        }

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
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class SchedulingPeriodContractValidShifts
    {

        private string shiftField;

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
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class SchedulingPeriodContractMinRestTime
    {

        private string labelField;

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
    public partial class SchedulingPeriodEmployee
    {

        private string[] contractIDField;

        private string idField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("ContractID")]
        public string[] ContractID
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
    public partial class SchedulingPeriodEmployee1
    {

        private string employeeIDField;

        private SchedulingPeriodEmployeeAssign assignField;

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
        public SchedulingPeriodEmployeeAssign Assign
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

        private byte dayField;

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
        public byte Day
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
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class SchedulingPeriodShiftOff
    {

        private string shiftField;

        private string employeeIDField;

        private byte dayField;

        private byte weightField;

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
        public byte Day
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
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte weight
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
    public partial class SchedulingPeriodShiftOn
    {

        private string shiftField;

        private string employeeIDField;

        private byte dayField;

        private byte weightField;

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
        public byte Day
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
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte weight
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
    public partial class SchedulingPeriodDateSpecificCover
    {

        private byte dayField;

        private SchedulingPeriodDateSpecificCoverCover coverField;

        /// <remarks/>
        public byte Day
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
        public SchedulingPeriodDateSpecificCoverCover Cover
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
    public partial class SchedulingPeriodDateSpecificCoverCover
    {

        private string shiftField;

        private SchedulingPeriodDateSpecificCoverCoverMin minField;

        private SchedulingPeriodDateSpecificCoverCoverMax maxField;

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
        public SchedulingPeriodDateSpecificCoverCoverMin Min
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
        public SchedulingPeriodDateSpecificCoverCoverMax Max
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
    public partial class SchedulingPeriodDateSpecificCoverCoverMin
    {

        private byte weightField;

        private byte valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte weight
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
    public partial class SchedulingPeriodDateSpecificCoverCoverMax
    {

        private byte weightField;

        private byte valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte weight
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


}
