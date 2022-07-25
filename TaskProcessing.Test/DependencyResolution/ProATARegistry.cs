using Lamar;
using ProATA.SharedKernel.Interfaces;

namespace TaskProcessing.Test.DependencyResolution
{
    public class ProATARegistry : ServiceRegistry
    {
        public ProATARegistry()
        {
            Scan(_ =>
            {
                _.Assembly("ProATA.SharedKernel");
                _.Assembly("TaskProcessing.Core");
                _.WithDefaultConventions();
                _.ConnectImplementationsToTypesClosing(typeof(IHandle<>));
            });
        }
    }
}
