import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatToolbarModule } from '@angular/material/toolbar';
import { ProdutoService } from '../../services/produto';
import { Auth } from '../../services/auth';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule, MatCardModule, MatButtonModule, MatIconModule, MatToolbarModule],
  templateUrl: './dashboard.html',
  styleUrl: './dashboard.scss'
})
export class Dashboard implements OnInit {
  totalProdutos = 0;
  role: string | null = null;

  constructor(
    private produtoService: ProdutoService,
    private auth: Auth,
    private router: Router,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit() {
    this.role = this.auth.getRole();
    this.produtoService.getAll().subscribe(p => {
      this.totalProdutos = p.length;
      this.cdr.detectChanges();
    });
  }

  logout() { this.auth.logout(); }
  irPara(rota: string) { this.router.navigate([rota]); }
}