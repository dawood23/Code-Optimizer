export interface CodeOptimizationRequest {
  language: string;
  code: string;
}

export interface CodeOptimizationResponse {
  optimizedCode: string;
  description: string;
  errorsFound: string[];
  optimizationsApplied: string[];
}

export interface Language {
  value: string;
  label: string;
  icon: string;
}