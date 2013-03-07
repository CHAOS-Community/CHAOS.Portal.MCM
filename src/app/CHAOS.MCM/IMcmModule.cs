namespace Chaos.Mcm
{
    using Chaos.Mcm.Data;
    using Chaos.Mcm.Permission;
    using Chaos.Portal.Module;

    public interface IMcmModule : IModule
    {
        IMcmRepository McmRepository { get; }

        IPermissionManager PermissionManager { get; }
    }
}