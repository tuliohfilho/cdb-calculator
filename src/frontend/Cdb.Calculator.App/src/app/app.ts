import { Component, inject } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { FormsModule } from '@angular/forms'; // Import FormsModule
import { CommonModule } from '@angular/common'; // Import CommonModule for *ngIf etc.
import { catchError } from 'rxjs/operators';
import { throwError } from 'rxjs';

interface CalculationResult {
  grossResult: number;
  netResult: number;
}

interface CalculationRequest {
  initialValue: number;
  months: number;
}

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [FormsModule, CommonModule],
  templateUrl: './app.html',
  styleUrls: ['./app.css']
})
export class AppComponent {
  private http = inject(HttpClient);

  initialValue: number | null = null;
  months: number | null = null;
  calculationResult: CalculationResult | null = null;
  errorMessage: string | null = null;

  // private apiUrl = 'https://localhost:7131/api/Cdb/calculate'; <- para rodar localmente
  private apiUrl = 'http://cdbcalculator-api:8080/api/Cdb/calculate'; // URL do backend no Docker

  calculateCdb(): void {
    this.calculationResult = null;
    this.errorMessage = null;

    if (this.initialValue === null || this.months === null || this.initialValue <= 0 || this.months <= 1) {
        this.errorMessage = 'Por favor, insira um valor inicial positivo e um prazo em meses maior que 1.';
        return;
    }

    const request: CalculationRequest = {
        initialValue: this.initialValue,
        months: this.months
    };

    this.http.post<CalculationResult>(this.apiUrl, request)
      .pipe(
        catchError((error: HttpErrorResponse) => {
          console.error('API Error:', error);
          if (error.status === 400 && error.error?.message) {
            this.errorMessage = `Erro na validação: ${error.error.message}`;
          } else if (error.status === 0) {
             this.errorMessage = 'Não foi possível conectar à API. Verifique se o backend está rodando e o CORS está configurado.';
          } else {
            this.errorMessage = `Ocorreu um erro ao calcular: ${error.statusText || 'Erro desconhecido'}`;
          }
          return throwError(() => new Error('Calculation failed'));
        })
      )
      .subscribe(result => {
        this.calculationResult = result;
      });
  }
}