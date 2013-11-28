namespace Chaos.Mcm
{
    using Chaos.Mcm.Data;
    using Chaos.Mcm.Permission;
    using Chaos.Portal.Core.Module;
    using Configuration;

    public interface IMcmModule : IModule
    {
        IMcmRepository McmRepository { get; }

        IPermissionManager PermissionManager { get; }
        McmModuleConfiguration McmModuleConfiguration { get; set; }
    }
}