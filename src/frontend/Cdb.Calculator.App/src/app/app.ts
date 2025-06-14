import { Component, inject } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
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

  initialValueText: string = '';
  initialValue: number | null = null;
  months: number | null = null;
  calculationResult: CalculationResult | null = null;
  errorMessage: string | null = null;

  private apiUrl = 'http://localhost:5137/api/Cdb/calculate';

  parseInitialValue(): void {
    if (!this.initialValueText) {
      this.initialValue = null;
      return;
    }

    let numericString = this.initialValueText.replace(/[^\d,]/g, '');

    numericString = numericString.replace(',', '.');

    const parsed = parseFloat(numericString);

    this.initialValue = isNaN(parsed) ? null : parsed;
  }

  formatInitialValue(): void {
    if (this.initialValue === null) {
      this.initialValueText = '';
      return;
    }

    this.initialValueText = this.initialValue.toLocaleString('pt-BR', {
      minimumFractionDigits: 2,
      maximumFractionDigits: 2
    });
  }

  validMonthsValue(): void {
    if (this.months === null || this.months <= 1) {
      this.months = null;
      this.errorMessage = 'Por favor, insira um valor inicial positivo e um prazo em meses maior que 1.';
      return;
    }

    this.months = Math.floor(this.months);
    this.errorMessage = '';
  }

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