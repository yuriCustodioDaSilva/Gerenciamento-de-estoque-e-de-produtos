import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';

export interface MovimentacaoDto {
  produtoId: number;
  quantidade: number;
  observacao: string;
}

@Injectable({ providedIn: 'root' })
export class EstoqueService {
  private apiUrl = `${environment.apiUrl}/estoque`;

  constructor(private http: HttpClient) {}

  entrada(dto: MovimentacaoDto) { return this.http.post(`${this.apiUrl}/entrada`, dto); }
  saida(dto: MovimentacaoDto) { return this.http.post(`${this.apiUrl}/saida`, dto); }
  reserva(dto: MovimentacaoDto) { return this.http.post(`${this.apiUrl}/reserva`, dto); }
  cancelarReserva(dto: MovimentacaoDto) { return this.http.post(`${this.apiUrl}/cancelar-reserva`, dto); }
  ajuste(dto: MovimentacaoDto) { return this.http.post(`${this.apiUrl}/ajuste`, dto); }
  historico() { return this.http.get<any[]>(`${this.apiUrl}/historico`); }
  historicoPorProduto(produtoId: number) { return this.http.get<any[]>(`${this.apiUrl}/historico/${produtoId}`); }
}