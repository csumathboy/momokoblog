using csumathboy.Shared.Notifications;

namespace csumathboy.Client.Infrastructure.Notifications;
public record ConnectionStateChanged(ConnectionState State, string? Message) : INotificationMessage;