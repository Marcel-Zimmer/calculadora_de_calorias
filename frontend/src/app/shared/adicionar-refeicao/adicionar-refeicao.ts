import { Component, inject, model, signal, output, computed } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RefeicaoService } from '../../core/services/refeicao.service';
import { AutenticacaoService } from '../../core/services/autenticacao.service';

@Component({
  selector: 'app-adicionar-refeicao',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './adicionar-refeicao.html',
  styleUrl: './adicionar-refeicao.css',
})
export class AdicionarRefeicao {
  
  // Comunicação com o Componente Pai (Dashboard)
  mostrarModal = model<boolean>(false);
  refeicaoAdicionada = output<void>();

  // Injeção de Dependências
  refeicaoService = inject(RefeicaoService);
  autenticacao = inject(AutenticacaoService);

  // Variáveis de Estado (Signals)
  todayDate = new Date().toLocaleDateString('en-CA', { timeZone: 'America/Sao_Paulo' });
  imagemRefeicao = signal<string | null>(null);
  
  obterTipoRefeicaoAtual(): number {
    const dataBrasilia = new Date(new Date().toLocaleString("en-US", { timeZone: "America/Sao_Paulo" }));
    const hora = dataBrasilia.getHours();
    if (hora < 11) return 1; // Café da Manhã
    if (hora < 15) return 2; // Almoço
    if (hora < 19) return 4; // Lanche
    return 3; // Jantar
  }

  // Campos do Formulário
  tipoRefeicao = signal<number>(this.obterTipoRefeicaoAtual());
  dataRefeicao = signal<string>(this.todayDate);
  pesoRefeicao = signal<number | null>(null);
  apelidoRefeicao = signal<string>('');
  fotoReal = signal<File | null>(null);

  // Novos campos para modelos
  usarModelo = signal<boolean>(false);
  modelosFrequentes = signal<any[]>([]);
  idModeloSelecionado = signal<number | null>(null);

  // Cálculo de calorias estimadas baseado no modelo selecionado
  caloriasEstimadas = computed(() => {
    const id = this.idModeloSelecionado();
    const peso = this.pesoRefeicao();
    
    if (id && peso) {
      const modelo = this.modelosFrequentes().find(m => m.id == id);
      if (modelo && modelo.calorias && modelo.pesoOriginal) {
        return Math.round((modelo.calorias / modelo.pesoOriginal) * peso);
      }
    }
    return null;
  });

  // Mapeamentos Visuais
  mapaRefeicoes: Record<number, any> = {
    1: { nome: 'Café da Manhã', icone: '☕', cor: 'bg-orange-100 text-orange-500' },
    2: { nome: 'Almoço',        icone: '🍽️', cor: 'bg-emerald-100 text-emerald-500' },
    3: { nome: 'Jantar',        icone: '🌙', cor: 'bg-blue-100 text-blue-500' },
    4: { nome: 'Lanche',        icone: '🥪', cor: 'bg-purple-100 text-purple-500' }
  }; 

  carregarModelos() {
    this.refeicaoService.obterModelosFrequentes(this.autenticacao.obterId()).subscribe({
      next: (modelos) => this.modelosFrequentes.set(modelos),
      error: (err) => console.error('Erro ao carregar modelos', err)
    });
  }

  toggleUsarModelo(valor: boolean) {
    this.usarModelo.set(valor);
    if (valor && this.modelosFrequentes().length === 0) {
      this.carregarModelos();
    }
  }

  closeMealModal() {
    this.mostrarModal.set(false);
    this.imagemRefeicao.set(null);
  }

  onFileSelected(event: any) {
    const file = event.target.files[0];
    if (file) {
      this.fotoReal.set(file);

      const reader = new FileReader();
      reader.onload = (e: any) => this.imagemRefeicao.set(e.target.result);
      reader.readAsDataURL(file);
    }
  }

  salvarRefeicao() {
    let novaRefeicao: FormData = new FormData();
    novaRefeicao.append("Tipo", this.tipoRefeicao().toString());
    novaRefeicao.append("Data", this.dataRefeicao());
    novaRefeicao.append("PesoEmGramas", this.pesoRefeicao()?.toString() ?? "");
    novaRefeicao.append("UsuarioId", this.autenticacao.obterId().toString());
    
    if (this.usarModelo() && this.idModeloSelecionado()) {
      novaRefeicao.append("CodigoRefeicaoModelo", this.idModeloSelecionado()!.toString());
    } else {
      novaRefeicao.append("Apelido", this.apelidoRefeicao());
      const foto = this.fotoReal();
      if (foto) {
        novaRefeicao.append("Imagem", foto);
      }
    }

    this.refeicaoService.adicionar(novaRefeicao).subscribe({
      next: (resposta: any) => {
        console.log('Cadastrado com sucesso!', resposta);
        
        // Avisa o Dashboard para recarregar os gráficos!
        this.refeicaoAdicionada.emit();
        
        // Limpa os campos e fecha a modal
        this.limparEFecharModal();
      },
      error: (erro) => {
        console.error('Falha ao salvar refeição', erro);
      }
    });
  }

  limparEFecharModal() {
    this.tipoRefeicao.set(this.obterTipoRefeicaoAtual());
    this.dataRefeicao.set(this.todayDate);
    this.pesoRefeicao.set(null);
    this.apelidoRefeicao.set('');
    this.fotoReal.set(null);
    this.idModeloSelecionado.set(null);
    this.usarModelo.set(false);
    this.closeMealModal();
  } 
}