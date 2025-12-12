import { Component, inject, OnInit, ViewEncapsulation } from '@angular/core';
import { AuthService } from '../../services/auth/auth.service';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-home-page',
  imports: [CommonModule],
  templateUrl: './home-page.component.html',
  styleUrls: ['./home-page.component.scss'],
  encapsulation:ViewEncapsulation.None
})
export class HomePageComponent implements OnInit {
  private authService = inject(AuthService);
  private router = inject(Router);

  currentYear = new Date().getFullYear();
  stats = {
    optimizedCode: 127,
    performanceImprovement: 42,
    activeUsers: 1567
  };

  isOptimizing = false;
  recentOptimizations = [
    { fileName: 'app.component.ts', timeAgo: '2 hours ago' },
    { fileName: 'auth.service.ts', timeAgo: '1 day ago' },
    { fileName: 'dashboard.component.ts', timeAgo: '3 days ago' }
  ];

  showToast = false;
  toastType: 'success' | 'warning' = 'success';
  toastTitle = '';
  toastMessage = '';
  private toastTimeout: any;

  ngOnInit() {
    this.loadUserData();
  }

  getUserInitials(): string {
    return 'JD';
  }

  loadUserData() {
    // Load user-specific data
    // Example: this.authService.getUserStats().subscribe(stats => this.stats = stats);
  }

  optimizeCode() {
    this.isOptimizing = true;

    this.router.navigate(['/code-optimizer']);
   
  }

  logout() {
    this.showToastMessage(
      'Logging Out', 
      'You are being logged out. Redirecting to login page...', 
      'warning'
    );
    
    setTimeout(() => {
      this.authService.logout();
      this.router.navigate(['/login']);
    }, 1500);
  }

  viewHistory() {
    this.router.navigate(['/history']);
  }

  openSettings() {
    this.router.navigate(['/settings']);
  }

  uploadFile() {
    this.showToastMessage('Upload Feature', 'File upload functionality coming soon!', 'success');
  }

  scanProject() {
    this.showToastMessage('Project Scan', 'Project scanning functionality coming soon!', 'success');
  }

  viewReports() {
    this.router.navigate(['/reports']);
  }

  openDocumentation() {
    window.open('https://docs.codeoptimizer.com', '_blank');
  }

  showToastMessage(title: string, message: string, type: 'success' | 'warning' = 'success') {
    this.toastTitle = title;
    this.toastMessage = message;
    this.toastType = type;
    this.showToast = true;

    if (this.toastTimeout) {
      clearTimeout(this.toastTimeout);
    }
    
    this.toastTimeout = setTimeout(() => {
      this.hideToast();
    }, 5000);
  }

  hideToast() {
    this.showToast = false;
    if (this.toastTimeout) {
      clearTimeout(this.toastTimeout);
    }
  }
}