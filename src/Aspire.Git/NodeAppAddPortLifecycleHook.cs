using Aspire.Hosting;
using Aspire.Hosting.ApplicationModel;
using Aspire.Hosting.Lifecycle;

namespace Aspire.Git;

internal class NodeAppAddPortLifecycleHook : IDistributedApplicationLifecycleHook
{
    public Task BeforeStartAsync(DistributedApplicationModel distributedApplicationModel, CancellationToken cancellationToken = default)
    {
        IEnumerable<NodeAppResource> nodeApps = distributedApplicationModel.Resources.OfType<NodeAppResource>();

        foreach (NodeAppResource app in nodeApps)
        {
            if (!app.TryGetEndpoints(out IEnumerable<EndpointAnnotation>? bindings))
            {
                continue;
            }

            EnvironmentCallbackAnnotation envAnnotation = new(env =>
            {
                bool multiBindings = bindings.Count() > 1;

                if (multiBindings)
                {
                    foreach (EndpointAnnotation binding in bindings)
                    {
                        string serviceName = multiBindings ? $"{app.Name}_{binding.Name}" : app.Name;
                        env[$"PORT_{binding.Name.ToUpperInvariant()}"] = $"{{{{- portForServing \"{serviceName}\" -}}}}";
                    }
                }
                else
                {
                    env["PORT"] = $"{{{{- portForServing \"{app.Name}\" -}}}}";
                }
            });

            app.Annotations.Add(envAnnotation);
        }

        return Task.CompletedTask;
    }
}