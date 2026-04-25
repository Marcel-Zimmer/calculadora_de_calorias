using Microsoft.AspNetCore.SignalR;

namespace CalculadoraCalorias.Api.Hubs
{
    public class NotificacaoHub : Hub
    {
        public async Task RegistrarCliente(string usuarioId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, usuarioId);
            Console.WriteLine($"Cliente conectado ao grupo do usuário: {usuarioId}");
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            // O SignalR gerencia a remoção automática de grupos ao desconectar, 
            // mas podemos logar se necessário.
            await base.OnDisconnectedAsync(exception);
        }
    }
}
