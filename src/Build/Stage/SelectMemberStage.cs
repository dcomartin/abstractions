
namespace Unity.Build.Stage
{

    /// <summary>
    /// The stages used in injection members selection pipeline.
    /// </summary>
    public enum SelectMemberStage
    {
        /// <summary>
        /// First stage. By default, nothing happens here.
        /// </summary>
        Setup,

        /// <summary>
        /// Stage where injected members are checked
        /// </summary>
        Injection,

        /// <summary>
        /// Stage where members are checked for appropriate 
        /// injection attributes.
        /// </summary>
        Attrubute,

        /// <summary>
        /// Members are reflected and selected.
        /// </summary>
        Reflection
    }
}
