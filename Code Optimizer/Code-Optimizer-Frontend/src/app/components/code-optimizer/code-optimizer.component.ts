import { Component, OnInit, signal, computed, inject} from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ApiService } from '../../services/api/api.service';
import { CodeOptimizationRequest, CodeOptimizationResponse, Language } from '../../models/code-optimization.model';
import { CodeFormatterService } from '../../services/formatting/code-formatter.service';
import { MonacoEditorModule } from 'ngx-monaco-editor-v2';

@Component({
  selector: 'app-code-optimizer',
  standalone: true,
  imports: [FormsModule,MonacoEditorModule],
  templateUrl: './code-optimizer.component.html',
  styleUrls: ['./code-optimizer.component.scss']
})
export class CodeOptimizerComponent implements OnInit {
  languages = signal<Language[]>([]);
  selectedLanguage = signal<string>('javascript');
  inputCode = signal<string>('');
  outputCode = signal<string>('');
  description = signal<string>('');
  errorsFound = signal<string[]>([]);
  optimizationsApplied = signal<string[]>([]);
  isLoading = signal<boolean>(false);
  error = signal<string>('');

  codeFormatterService = inject(CodeFormatterService);
  hasResults = computed(() => 
    this.errorsFound().length > 0 || 
    this.optimizationsApplied().length > 0
  );

  hasOutput = computed(() => this.outputCode().length > 0);

  canOptimize = computed(() => 
    !this.isLoading() && this.inputCode().trim().length > 0
  );

  constructor(private codeOptimizerService: ApiService) { }

  ngOnInit(): void {
    this.languages.set(this.codeOptimizerService.getLanguageOptions());
    this.loadExampleCode();
  }

  onLanguageChange(newLanguage: string): void {
    this.selectedLanguage.set(newLanguage);
    this.loadExampleCode();
    this.clearResults();
  }

  loadExampleCode(): void {
    const example = this.codeOptimizerService.getExampleCode(this.selectedLanguage());
    this.inputCode.set(example);
  }

  optimizeCode(): void {
    if (!this.inputCode().trim()) {
      this.error.set('Please enter code to optimize');
      return;
    }

    this.isLoading.set(true);
    this.error.set('');
    this.clearResults();

    const request: CodeOptimizationRequest = {
      language: this.selectedLanguage(),
      code: this.inputCode()
    };

    this.codeOptimizerService.optimizeCode(request).subscribe({
      next: (response: CodeOptimizationResponse) => {
        this.codeFormatterService.format(response.optimizedCode,this.selectedLanguage()).then((formattedCode: string) => {
          this.outputCode.set(formattedCode);
        })
        this.description.set(response.description);
        this.errorsFound.set(response.errorsFound || []);
        this.optimizationsApplied.set(response.optimizationsApplied || []);
        this.isLoading.set(false);
      },
      error: (err) => {
        const serverError = err.error;

        let message = serverError?.error || 'An error occurred while optimizing code';
        if (serverError?.details) {
          try {
            const match = serverError.details.match(/message":"([^"]+)"/);
            if (match?.[1]) {
              message += ': ' + match[1];
            }
          } catch {}
        }

        this.error.set(message);

        this.isLoading.set(false);
      }
    });
  }

  clearResults(): void {
    this.outputCode.set('');
    this.description.set('');
    this.errorsFound.set([]);
    this.optimizationsApplied.set([]);
    this.error.set('');
  }

  clearAll(): void {
    this.inputCode.set('');
    this.clearResults();
  }

  copyToClipboard(text: string): void {
    navigator.clipboard.writeText(text).then(() => {
      console.log('Copied to clipboard!');
    });
  }
  updateInputCode(value: string): void {
    this.inputCode.set(value);
  }

  updateOutputCode(value: string): void {
    this.outputCode.set(value);
  }
}