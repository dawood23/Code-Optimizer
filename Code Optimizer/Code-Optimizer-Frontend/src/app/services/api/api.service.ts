import { Injectable } from '@angular/core';
import { env } from '../../Environment/env';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { CodeOptimizationRequest,CodeOptimizationResponse,Language } from '../../models/code-optimization.model';

@Injectable({
  providedIn: 'root',
})
export class ApiService {
   private apiUrl = env.apiBaseUrl; 

  constructor(private http: HttpClient) { }

  optimizeCode(request: CodeOptimizationRequest): Observable<CodeOptimizationResponse> {
    return this.http.post<CodeOptimizationResponse>(`${this.apiUrl}/api/CodeOptimizer/optimize`, request);
  }

  getSupportedLanguages(): Observable<string[]> {
    return this.http.get<string[]>(`${this.apiUrl}/api/CodeOptimizer/supported-languages`);
  }

  getLanguageOptions(): Language[] {
    return [
      { value: 'javascript', label: 'JavaScript', icon: '📜' },
      { value: 'typescript', label: 'TypeScript', icon: '📘' },
      { value: 'csharp', label: 'C#', icon: '🔷' },
      { value: 'sql', label: 'SQL', icon: '🗄️' },
      { value: 'html', label: 'HTML', icon: '🌐' }
    ];
  }

  getExampleCode(language: string): string {
    const examples: { [key: string]: string } = {
      javascript: `function calculateTotal(items) {
  var total = 0;
  for (var i = 0; i < items.length; i++) {
    total = total + items[i].price;
  }
  return total;
}`,
      typescript: `function getUserName(user: any): string {
  if (user != null) {
    return user.name;
  }
  return "";
}`,
      csharp: `public class Calculator {
  public int Add(int a, int b) {
    int result = a + b;
    return result;
  }
}`,
      sql: `SELECT * FROM users 
WHERE status = 'active' 
   OR status = 'pending' 
   OR status = 'new';`,
      html: `<div class="container">
  <div class="row">
    <div class="col">
      <p>Hello World</p>
    </div>
  </div>
</div>`
    };
    return examples[language] || '';
  }
}
