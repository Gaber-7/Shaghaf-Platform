import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import {
  LoginRequest,
  RegisterRequest,
  AuthResponse,
  UserDto,
  RefreshTokenRequest,
  ChangePasswordRequest,
  CourseListItemDto,
  CourseDetailDto,
  CourseQuery,
  PagedResult,
  EnrollmentDto,
  CreateEnrollmentRequest,
  LessonProgressDto
} from '@app/shared/models';

@Injectable({
  providedIn: 'root'
})
export class ApiService {
  private readonly apiUrl = 'http://localhost:5000/api';

  constructor(private http: HttpClient) { }

  // ============ Auth Endpoints ============
  login(request: LoginRequest): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(`${this.apiUrl}/auth/login`, request);
  }

  register(request: RegisterRequest): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(`${this.apiUrl}/auth/register`, request);
  }

  refresh(request: RefreshTokenRequest): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(`${this.apiUrl}/auth/refresh`, request);
  }

  logout(): Observable<void> {
    return this.http.post<void>(`${this.apiUrl}/auth/logout`, {});
  }

  changePassword(request: ChangePasswordRequest): Observable<void> {
    return this.http.post<void>(`${this.apiUrl}/auth/change-password`, request);
  }

  getCurrentUser(): Observable<UserDto> {
    return this.http.get<UserDto>(`${this.apiUrl}/auth/me`);
  }

  // ============ Course Endpoints ============
  searchCourses(query: CourseQuery): Observable<PagedResult<CourseListItemDto>> {
    let params = new HttpParams();
    if (query.searchTerm) params = params.set('searchTerm', query.searchTerm);
    if (query.category) params = params.set('category', query.category);
    if (query.level) params = params.set('level', query.level);
    if (query.page) params = params.set('page', query.page.toString());
    if (query.pageSize) params = params.set('pageSize', query.pageSize.toString());
    if (query.sortBy) params = params.set('sortBy', query.sortBy);
    if (query.sortOrder) params = params.set('sortOrder', query.sortOrder);

    return this.http.get<PagedResult<CourseListItemDto>>(`${this.apiUrl}/courses`, { params });
  }

  getCourseById(courseId: string): Observable<CourseDetailDto> {
    return this.http.get<CourseDetailDto>(`${this.apiUrl}/courses/${courseId}`);
  }

  // ============ Enrollment Endpoints ============
  enrollInCourse(request: CreateEnrollmentRequest): Observable<EnrollmentDto> {
    return this.http.post<EnrollmentDto>(`${this.apiUrl}/enrollments`, request);
  }

  getEnrollments(): Observable<EnrollmentDto[]> {
    return this.http.get<EnrollmentDto[]>(`${this.apiUrl}/enrollments`);
  }

  // ============ Lesson Progress Endpoints ============
  getLessonProgress(lessonId: string): Observable<LessonProgressDto> {
    return this.http.get<LessonProgressDto>(`${this.apiUrl}/lessons/${lessonId}/progress`);
  }

  updateLessonProgress(lessonId: string, watchedDuration: number): Observable<LessonProgressDto> {
    return this.http.post<LessonProgressDto>(
      `${this.apiUrl}/lessons/${lessonId}/progress`,
      { watchedDuration }
    );
  }

  markLessonComplete(lessonId: string): Observable<LessonProgressDto> {
    return this.http.post<LessonProgressDto>(
      `${this.apiUrl}/lessons/${lessonId}/complete`,
      {}
    );
  }
}
