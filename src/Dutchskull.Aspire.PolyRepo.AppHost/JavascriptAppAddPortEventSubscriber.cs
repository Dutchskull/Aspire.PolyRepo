using Aspire.Hosting;
using Aspire.Hosting.ApplicationModel;
using Aspire.Hosting.Eventing;
using Aspire.Hosting.JavaScript;
using Aspire.Hosting.Lifecycle;

namespace Dutchskull.Aspire.PolyRepo.AppHost;

internal class JavascriptAppAddPortEventSubscriber : IDistributedApplicationEventingSubscriber
{
    public Task SubscribeAsync(IDistributedApplicationEventing eventing, DistributedApplicationExecutionContext executionContext, CancellationToken cancellationToken)
    {
        eventing.Subscribe<BeforeStartEvent>((@event, cancellationToken) =>
        {
            DistributedApplicationModel model = @event.Model;
            var javascriptApps = model.Resources.OfType<JavaScriptAppResource>();

            foreach (var app in javascriptApps)
            {
                if (!app.TryGetEndpoints(out IEnumerable<EndpointAnnotation>? bindings))
                {
                    continue;
                }

                var endpointList = bindings.ToList();

                var envAnnotation = new EnvironmentCallbackAnnotation(env =>
                {
                    bool multiBindings = endpointList.Count > 1;

                    if (multiBindings)
                    {
                        foreach (EndpointAnnotation binding in endpointList)
                        {
                            string serviceName =
                                $"{app.Name}_{binding.Name}";

                            env[$"PORT_{binding.Name.ToUpperInvariant()}"] =
                                $"{{{{- portForServing \"{serviceName}\" -}}}}";
                        }
                    }
                    else
                    {
                        env["PORT"] =
                            $"{{{{- portForServing \"{app.Name}\" -}}}}";
                    }
                });

                app.Annotations.Add(envAnnotation);
            }

            return Task.CompletedTask;
        });

        return Task.CompletedTask;
    }
}