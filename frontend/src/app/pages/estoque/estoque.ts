import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { MatCardModule } from '@angular/material/card';
import { MatTabsModule } from '@angular/material/tabs';
import { EstoqueService } from '../../services/estoque';
import { ProdutoService, Produto } from '../../services/produto';
import { Auth } from '../../services/auth';

@Component({
  selector: 'app-estoque',
  standalone: true,
  imports: [CommonModule, FormsModule, MatTableModule, MatButtonModule, MatIconModule,
    MatInputModule, MatSelectModule, MatToolbarModule, MatSnackBarModule, MatCardModule, MatTabsModule],
  templateUrl: './estoque.html',
  styleUrl: './estoque.scss'
})

export class Estoque implements OnInit {
  produtos: Produto[] = [];
  historico: any[] = [];
  historicoColumns = ['produto', 'tipo', 'quantidade', 'usuario', 'data'];
  role: string | null = null;
  form = { produtoId: 0, quantidade: 0, observacao: '' };

  constructor(
    private estoqueService: EstoqueService,
    private produtoService: ProdutoService,
    private auth: Auth,
    private router: Router,
    private snackBar: MatSnackBar,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit() {
    this.role = this.auth.getRole();
    this.carregarProdutos();
    this.carregarHistorico();
  }

  carregarProdutos() {
    this.produtoService.getAll().subscribe(p => {
      this.produtos = p;
      this.cdr.detectChanges();
    });
  }

  carregarHistorico() {
    this.estoqueService.historico().subscribe(h => {
      this.historico = h;
      this.cdr.detectChanges();
    });
  }

  executar(tipo: string) {
    if (!this.form.produtoId || !this.form.quantidade) {
      this.snackBar.open('Preencha produto e quantidade', 'Fechar', { duration: 3000 });
      return;
    }
    const ops: any = {
      entrada: () => this.estoqueService.entrada(this.form),
      saida: () => this.estoqueService.saida(this.form),
      reserva: () => this.estoqueService.reserva(this.form),
      cancelar: () => this.estoqueService.cancelarReserva(this.form),
      ajuste: () => this.estoqueService.ajuste(this.form)
    };
    ops[tipo]().subscribe({
      next: () => {
        this.snackBar.open('Operação realizada!', 'Fechar', { duration: 3000 });
        this.carregarProdutos();
        this.carregarHistorico();
        this.form = { produtoId: 0, quantidade: 0, observacao: '' };
        this.cdr.detectChanges();
      },
      error: (err: any) => {
        let msg = 'Erro na operação';
        if (err.status === 403) {
          msg = 'Você não tem permissão para realizar esta operação';
        } else if (err.error?.message) {
          msg = err.error.message;
        } else if (typeof err.error === 'string') {
          msg = err.error;
        }
        this.snackBar.open(msg, 'Fechar', { duration: 3000 });
      }
    });
  }

  voltar() { this.router.navigate(['/dashboard']); }
}