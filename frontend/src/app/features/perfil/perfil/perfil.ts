import { Component, inject, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { PerfilBiometricoService } from '../../../core/services/perfil-biometrico.service';
import { RegistroFisicoService } from '../../../core/services/registro-fisico.service';
import { UsuarioService } from '../../../core/services/usuario.service';
import { AutenticacaoService } from '../../../core/services/autenticacao.service';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-perfil',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './perfil.html'
})
export class PerfilComponent implements OnInit {
  private perfilService = inject(PerfilBiometricoService);
  private registroFisicoService = inject(RegistroFisicoService);
  private usuarioService = inject(UsuarioService);
  private autenticacao = inject(AutenticacaoService);

  subAbaPerfil = signal<'biometrico' | 'fisico' | 'seguranca'>('biometrico');

  // Dados Biométricos
  perfilId = signal<number>(0);
  dataNascimento = signal<string>('');
  genero = signal<number>(1);
  alturaCm = signal<number>(0);
  nivelAtividade = signal<number>(1);
  objetivo = signal<number>(1);

  // Registro Físico
  registroFisicoId = signal<number>(0);
  pesoKg = signal<number>(0);
  metaCaloricaDiaria = signal<number>(0);

  // Segurança
  novaSenha = signal<string>('');
  confirmarNovaSenha = signal<string>('');

  ngOnInit(): void {
    this.carregarPerfil();
    this.carregarRegistroFisico();
  }

  carregarPerfil() {
    this.perfilService.obterPorUsuarioId(this.autenticacao.obterId()).subscribe((res: any) => {
      if (res) {
        this.perfilId.set(res.id);
        this.dataNascimento.set(res.dataNascimento.split('T')[0]);
        this.genero.set(res.genero);
        this.alturaCm.set(res.alturaCm);
        this.nivelAtividade.set(res.nivelAtividade);
        this.objetivo.set(res.objetivo);
      }
    });
  }

  carregarRegistroFisico() {
    this.registroFisicoService.obterUltimoPorUsuarioId(this.autenticacao.obterId()).subscribe((res: any) => {
      if (res) {
        this.registroFisicoId.set(res.id);
        this.pesoKg.set(res.pesoKg);
        this.metaCaloricaDiaria.set(res.metaCaloricaDiaria);
      }
    });
  }

  salvarPerfilBiometrico() {
    const dados = {
      usuarioId: this.autenticacao.obterId(),
      dataNascimento: this.dataNascimento(),
      genero: Number(this.genero()),
      alturaCm: Number(this.alturaCm()),
      nivelAtividade: Number(this.nivelAtividade()),
      objetivo: Number(this.objetivo())
    };

    const obs = this.perfilId() > 0 
      ? this.perfilService.atualizar(this.autenticacao.obterId(), { ...dados, id: this.perfilId() })
      : this.perfilService.adicionar(dados);

    obs.subscribe(() => {
      Swal.fire({ icon: 'success', title: 'Perfil atualizado!', showConfirmButton: false, timer: 1500 });
      this.carregarPerfil();
    });
  }

  salvarRegistroFisico() {
    const dados = {
      usuarioId: this.autenticacao.obterId(),
      pesoKg: Number(this.pesoKg()),
      metaCaloricaDiaria: Number(this.metaCaloricaDiaria())
    };

    const obs = this.registroFisicoId() > 0
      ? this.registroFisicoService.atualizar(this.autenticacao.obterId(), dados)
      : this.registroFisicoService.adicionar(dados);

    obs.subscribe(() => {
      Swal.fire({ icon: 'success', title: 'Metas atualizadas!', showConfirmButton: false, timer: 1500 });
      this.carregarRegistroFisico();
    });
  }

  salvarNovaSenha() {
    if (this.novaSenha() !== this.confirmarNovaSenha()) {
      Swal.fire('Erro', 'As senhas não coincidem', 'error');
      return;
    }
    this.usuarioService.atualizarSenha(this.novaSenha()).subscribe(() => {
      Swal.fire('Sucesso', 'Senha alterada!', 'success');
      this.novaSenha.set('');
      this.confirmarNovaSenha.set('');
    });
  }
}
