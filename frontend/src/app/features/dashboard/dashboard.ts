import { Component, computed, inject, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Menu } from '../../shared/menu/menu';
import { Ui } from '../../core/services/ui.service';
import { Carregamento } from '../../shared/carregamento/carregamento';


@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule, Carregamento],
  templateUrl: './dashboard.html',
  styleUrl: './dashboard.css'
})

export class Dashboard implements OnInit{

  ngOnInit(): void {

  }

  activeTab = signal<'dashboard' | 'estatisticas' | 'perfil'>('dashboard');
  isMenuOpen = signal<boolean>(false);
  timeView = signal<'diario' | 'semanal' | 'mensal'>('diario');
  
  showMealModal = signal<boolean>(false);
  showExerciseModal = signal<boolean>(false);

  mealImagePreview = signal<string | null>(null);
  todayDate = new Date().toISOString().split('T')[0];

  goalCalories = 2000;
  consumedCalories = 1450;
  burnedCalories = 300;
  
  // Médias Diárias Reais (Cálculo simulado)
  averageConsumed = computed(() => this.timeView() === 'semanal' ? 1920 : 2050);
  averageBurned = computed(() => this.timeView() === 'semanal' ? 250 : 220);

  // Totais do Período
  currentConsumed = computed(() => {
    if (this.timeView() === 'semanal') return 13440;
    if (this.timeView() === 'mensal') return 61500;
    return this.consumedCalories;
  });

  currentBurned = computed(() => {
     if (this.timeView() === 'semanal') return 1750;
     if (this.timeView() === 'mensal') return 6600;
     return this.burnedCalories;
  });

  weeklyChartData = [
    { label: 'SEG', kcal: 1900 }, { label: 'TER', kcal: 2300 },
    { label: 'QUA', kcal: 1850 }, { label: 'QUI', kcal: 2150 },
    { label: 'SEX', kcal: 1950 }, { label: 'SÁB', kcal: 1450 },
    { label: 'DOM', kcal: 0 }
  ];

  monthlyChartData = Array.from({length: 30}, (_, i) => ({
    label: (i + 1).toString(),
    kcal: i > 23 ? 0 : Math.floor(Math.random() * 800) + 1600 
  }));

  onFileSelected(event: any) {
    const file = event.target.files[0];
    if (file) {
      const reader = new FileReader();
      reader.onload = (e: any) => this.mealImagePreview.set(e.target.result);
      reader.readAsDataURL(file);
    }
  }

  closeMealModal() {
    this.showMealModal.set(false);
    this.mealImagePreview.set(null);
  }
}