
namespace Unity.Build.Stage
{
    /// <summary>
    /// The stages used in registration pipeline.
    /// </summary>
    public enum RegisterStage
    {
        /// <summary>
        /// First stage. By default, nothing happens here.
        /// </summary>
        Setup,

        /// <summary>
        /// Stage where Array or IEnumerable is resolved
        /// </summary>
        Collections,

        /// <summary>
        /// Third stage. lifetime managers are checked here,
        /// and if they're available the rest of the pipeline is skipped.
        /// </summary>
        Lifetime,

        /// <summary>
        /// Second stage. Type mapping occurs here.
        /// </summary>
        TypeMapping,

        /// <summary>
        /// Fourth stage. Reflection over constructors, properties, etc. is
        /// performed here.
        /// </summary>
        PreCreation,

        /// <summary>
        /// Fifth stage. Instance creation happens here.
        /// </summary>
        Creation,

        /// <summary>
        /// Sixth stage. Property sets and method injection happens here.
        /// </summary>
        Initialization,

        /// <summary>
        /// Seventh and final stage. By default, nothing happens here.
        /// </summary>
        PostInitialization
    }
}
