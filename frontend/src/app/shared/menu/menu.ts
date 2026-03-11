import { CommonModule } from '@angular/common';
import { Component, computed, inject, model, signal } from '@angular/core';
import { Ui } from '../../core/services/ui.service';
import { AutenticacaoService } from '../../core/services/autenticacao.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-menu',
  imports: [CommonModule],
  templateUrl: './menu.html',
  styleUrl: './menu.css',
})
export class Menu {
  ui = inject(Ui);
  constructor(private autenticacaoService:AutenticacaoService, private router: Router){};

  activeTab = signal<'dashboard' | 'estatisticas' | 'perfil'>('dashboard');
  menuAberto = model<boolean>(true);

  changeTabAndCloseSidebar(tab: 'dashboard' | 'estatisticas' | 'perfil') {
      this.activeTab.set(tab);
      this.menuAberto.set(false);
  }

  logout() {
    this.autenticacaoService.fazerLogout();
    this.router.navigate(['/login']);
  }
}
