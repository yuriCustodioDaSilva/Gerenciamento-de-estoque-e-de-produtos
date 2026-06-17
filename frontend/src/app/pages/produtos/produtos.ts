import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { MatCardModule } from '@angular/material/card';
import { Produto, ProdutoService } from '../../services/produto';
import { Auth } from '../../services/auth';

@Component({
  selector: 'app-produtos',
  standalone: true,
  imports: [CommonModule, FormsModule, MatTableModule, MatButtonModule, MatIconModule,
    MatInputModule, MatToolbarModule, MatSnackBarModule, MatCardModule],
  templateUrl: './produtos.html',
  styleUrl: './produtos.scss'
})
export class Produtos implements OnInit {
  produtos: Produto[] = [];
  displayedColumns = ['nome', 'sku', 'preco', 'quantidadeDisponivel', 'acoes'];
  role: string | null = null;
  showForm = false;
  editando = false;
  form: Produto = { nome: '', descricao: '', sku: '', preco: 0 };

  constructor(
    private produtoService: ProdutoService,
    private auth: Auth,
    private router: Router,
    private snackBar: MatSnackBar,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit() {
    this.role = this.auth.getRole();
    this.carregar();
  }

  carregar() {
    this.produtoService.getAll().subscribe(p => {
      this.produtos = p;
      this.cdr.detectChanges();
    });
  }

  novoForm() {
    this.form = { nome: '', descricao: '', sku: '', preco: 0 };
    this.editando = false;
    this.showForm = true;
  }

  editarForm(produto: Produto) {
    this.form = { ...produto };
    this.editando = true;
    this.showForm = true;
  }

  salvar() {
    if (this.editando && this.form.id) {
      this.produtoService.update(this.form.id, this.form).subscribe({
        next: () => { this.snackBar.open('Produto atualizado!', 'Fechar', { duration: 3000 }); this.carregar(); this.showForm = false; },
        error: () => this.snackBar.open('Erro ao atualizar', 'Fechar', { duration: 3000 })
      });
    } else {
      this.produtoService.create(this.form).subscribe({
        next: () => { this.snackBar.open('Produto criado!', 'Fechar', { duration: 3000 }); this.carregar(); this.showForm = false; },
        error: () => this.snackBar.open('Erro ao criar', 'Fechar', { duration: 3000 })
      });
    }
  }

  deletar(id: number) {
    if (confirm('Deseja deletar este produto?')) {
      this.produtoService.delete(id).subscribe({
        next: () => { this.snackBar.open('Produto deletado!', 'Fechar', { duration: 3000 }); this.carregar(); },
        error: () => this.snackBar.open('Erro ao deletar', 'Fechar', { duration: 3000 })
      });
    }
  }

  voltar() { this.router.navigate(['/dashboard']); }
}