import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';

export interface Produto {
  id?: number;
  nome: string;
  descricao: string;
  sku: string;
  preco: number;
  quantidadeEmEstoque?: number;
  quantidadeReservada?: number;
  quantidadeDisponivel?: number;
}

@Injectable({ providedIn: 'root' })
export class ProdutoService {
  private apiUrl = `${environment.apiUrl}/produtos`;

  constructor(private http: HttpClient) {}

  getAll() { return this.http.get<Produto[]>(this.apiUrl); }
  getById(id: number) { return this.http.get<Produto>(`${this.apiUrl}/${id}`); }
  create(produto: Produto) { return this.http.post<Produto>(this.apiUrl, produto); }
  update(id: number, produto: Produto) { return this.http.put(`${this.apiUrl}/${id}`, produto); }
  delete(id: number) { return this.http.delete(`${this.apiUrl}/${id}`); }
}