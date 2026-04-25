import { inject, Injectable, signal } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { environment } from '../../../environments/environment';
import { AutenticacaoService } from './autenticacao.service';
import { Subject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class NotificacaoService {
  private authService = inject(AutenticacaoService);
  private hubConnection: signalR.HubConnection | null = null;
  
  // Observable para o componente escutar
  public refeicaoProcessada$ = new Subject<number>();

  constructor() {
    // Inicia a conexão se o usuário já estiver logado
    if (this.authService.logado()) {
      this.iniciarConexao();
    }
  }

  public iniciarConexao() {
    if (this.hubConnection?.state === signalR.HubConnectionState.Connected) return;

    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(`${environment.apiUrl.replace('/api', '')}/api/hubs/notificacoes`, {
        skipNegotiation: true,
        transport: signalR.HttpTransportType.WebSockets
      })
      .withAutomaticReconnect()
      .build();

    this.hubConnection
      .start()
      .then(() => {
        console.log('Conectado ao SignalR Hub!');
        this.registrarNoGrupo();
      })
      .catch(err => console.error('Erro ao conectar ao SignalR:', err));

    // Escutar evento do servidor
    this.hubConnection.on('RefeicaoProcessada', (refeicaoId: number) => {
      console.log(`Refeição ${refeicaoId} processada com sucesso!`);
      this.refeicaoProcessada$.next(refeicaoId);
    });
  }

  private registrarNoGrupo() {
    const userId = this.authService.obterId();
    if (userId && this.hubConnection) {
      this.hubConnection.invoke('RegistrarCliente', userId.toString())
        .catch(err => console.error('Erro ao registrar no grupo:', err));
    }
  }

  public pararConexao() {
    this.hubConnection?.stop();
  }
}
