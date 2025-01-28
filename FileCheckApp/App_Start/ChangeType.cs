using System.ComponentModel;

namespace FileCheckApp.App_Start
{
    public enum ChangeType
    {
        [Description("No Change")]
        NoChange,
        [Description("New File")]
        NewFile,
        [Description("Modified")]
        Modified,
        [Description("Deleted")]
        Deleted
    }
}