import { Component, inject, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { Menu } from './shared/menu/menu';
import { AutenticacaoService } from './core/services/autenticacao.service';
import { Carregamento } from "./shared/carregamento/carregamento";

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, Menu, Carregamento],
  templateUrl: './app.html',
  styleUrl: './app.css'
})

export class App {
  protected readonly title = signal('frontend');
  autenticacao = inject(AutenticacaoService);
}
