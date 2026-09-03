import { Component, OnInit } from '@angular/core';
import { AuthService } from '@app/core/services/auth.service';
import { ApiService } from '@app/core/services/api.service';
import { UserDto, EnrollmentDto, CourseDetailDto } from '@app/shared/models';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent implements OnInit {
  currentUser: UserDto | null = null;
  studentName = 'Student';
  courseCount = 0;
  assignmentCount = 0;
  studyHours = 0;
  certificateCount = 0;
  progressPercentage = 0;

  courses = [
    { name: 'Web Development', progress: 85, lessons: 24 },
    { name: 'Mobile Apps', progress: 65, lessons: 18 },
    { name: 'Data Science', progress: 45, lessons: 20 }
  ];

  enrollments: EnrollmentDto[] = [];
  isLoading = true;
  errorMessage = '';

  constructor(
    private authService: AuthService,
    private apiService: ApiService
  ) { }

  ngOnInit(): void {
    this.loadDashboard();
  }

  private loadDashboard(): void {
    this.currentUser = this.authService.getCurrentUser();

    if (this.currentUser) {
      this.studentName = `${this.currentUser.firstName} ${this.currentUser.lastName}`;
    }

    this.apiService.getEnrollments().subscribe({
      next: (enrollments) => {
        this.enrollments = enrollments;
        this.courseCount = enrollments.length;
        this.updateStats();
        this.isLoading = false;
      },
      error: (error) => {
        console.error('Error loading enrollments:', error);
        this.errorMessage = 'Failed to load your courses';
        this.isLoading = false;
      }
    });
  }

  private updateStats(): void {
    // Calculate stats from enrollments
    this.progressPercentage = this.enrollments.length > 0
      ? Math.round(this.enrollments.reduce((sum, e) => sum + e.progress, 0) / this.enrollments.length)
      : 0;

    // TODO: Load these from API
    this.assignmentCount = 8;
    this.studyHours = 24;
    this.certificateCount = 5;
  }
}


