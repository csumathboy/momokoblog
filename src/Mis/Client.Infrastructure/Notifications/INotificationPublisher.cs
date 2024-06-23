using csumathboy.Shared.Notifications;

namespace csumathboy.Client.Infrastructure.Notifications;
public interface INotificationPublisher
{
    Task PublishAsync(INotificationMessage notification);
}