namespace Chaos.Mcm
{
    using Data;
    using Permission;
    using Portal.Core.Module;
    using Configuration;

    public interface IMcmModule : IModuleConfig
    {
        IMcmRepository McmRepository { get; }

        IPermissionManager PermissionManager { get; }
        McmModuleConfiguration Configuration { get; set; }
    }
}