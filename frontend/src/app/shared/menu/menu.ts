import { CommonModule } from '@angular/common';
import { Component, inject, input, model, output, signal } from '@angular/core';
import { Ui } from '../../core/services/ui.service';
import { AutenticacaoService } from '../../core/services/autenticacao.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-menu',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './menu.html',
  styleUrl: './menu.css',
})
export class Menu {
  ui = inject(Ui);
  private autenticacaoService = inject(AutenticacaoService);
  private router = inject(Router);

  abaAtiva = input<'dashboard' | 'estatisticas-consumo' | 'estatisticas-gasto' | 'perfil'>('dashboard');
  menuAberto = model<boolean>(false);
  abaSelecionada = output<'dashboard' | 'estatisticas-consumo' | 'estatisticas-gasto' | 'perfil'>();

  selecionarAba(aba: 'dashboard' | 'estatisticas-consumo' | 'estatisticas-gasto' | 'perfil') {
      this.abaSelecionada.emit(aba);
      this.menuAberto.set(false);
      this.ui.fecharMenu();
  }

  logout() {
    this.autenticacaoService.fazerLogout();
    this.router.navigate(['/login']);
  }
}
