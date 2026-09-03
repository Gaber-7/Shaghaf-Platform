import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, timer, throwError } from 'rxjs';
import { tap, switchMap, catchError } from 'rxjs/operators';
import { ApiService } from './api.service';
import {
  LoginRequest,
  RegisterRequest,
  AuthResponse,
  UserDto,
  RefreshTokenRequest,
  ChangePasswordRequest
} from '@app/shared/models';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private readonly tokenKey = 'access_token';
  private readonly refreshTokenKey = 'refresh_token';
  private readonly userKey = 'current_user';
  private readonly expiresInKey = 'token_expires_in';

  private currentUserSubject = new BehaviorSubject<UserDto | null>(this.getUserFromStorage());
  private isAuthenticatedSubject = new BehaviorSubject<boolean>(this.hasValidToken());

  public currentUser$ = this.currentUserSubject.asObservable();
  public isAuthenticated$ = this.isAuthenticatedSubject.asObservable();

  constructor(private apiService: ApiService) {
    this.setupTokenRefresh();
  }

  login(request: LoginRequest): Observable<AuthResponse> {
    return this.apiService.login(request).pipe(
      tap(response => this.handleAuthResponse(response)),
      catchError(error => {
        console.error('Login error:', error);
        return throwError(() => error);
      })
    );
  }

  register(request: RegisterRequest): Observable<AuthResponse> {
    return this.apiService.register(request).pipe(
      tap(response => this.handleAuthResponse(response)),
      catchError(error => {
        console.error('Register error:', error);
        return throwError(() => error);
      })
    );
  }

  logout(): void {
    this.apiService.logout().subscribe({
      next: () => this.clearAuth(),
      error: () => this.clearAuth()
    });
  }

  changePassword(request: ChangePasswordRequest): Observable<void> {
    return this.apiService.changePassword(request).pipe(
      catchError(error => {
        console.error('Change password error:', error);
        return throwError(() => error);
      })
    );
  }

  refreshToken(): Observable<AuthResponse> {
    const refreshToken = this.getRefreshToken();
    if (!refreshToken) {
      return throwError(() => new Error('No refresh token available'));
    }

    return this.apiService.refresh({ refreshToken }).pipe(
      tap(response => this.handleAuthResponse(response)),
      catchError(error => {
        this.clearAuth();
        return throwError(() => error);
      })
    );
  }

  getCurrentUser(): UserDto | null {
    return this.currentUserSubject.value;
  }

  isAuthenticated(): boolean {
    return this.hasValidToken();
  }

  getAccessToken(): string | null {
    return localStorage.getItem(this.tokenKey);
  }

  private getRefreshToken(): string | null {
    return localStorage.getItem(this.refreshTokenKey);
  }

  private handleAuthResponse(response: AuthResponse): void {
    localStorage.setItem(this.tokenKey, response.accessToken);
    localStorage.setItem(this.refreshTokenKey, response.refreshToken);
    localStorage.setItem(this.expiresInKey, response.expiresIn.toString());
    localStorage.setItem(this.userKey, JSON.stringify(response.user));

    this.currentUserSubject.next(response.user);
    this.isAuthenticatedSubject.next(true);
  }

  private clearAuth(): void {
    localStorage.removeItem(this.tokenKey);
    localStorage.removeItem(this.refreshTokenKey);
    localStorage.removeItem(this.expiresInKey);
    localStorage.removeItem(this.userKey);

    this.currentUserSubject.next(null);
    this.isAuthenticatedSubject.next(false);
  }

  private getUserFromStorage(): UserDto | null {
    const user = localStorage.getItem(this.userKey);
    return user ? JSON.parse(user) : null;
  }

  private hasValidToken(): boolean {
    const token = localStorage.getItem(this.tokenKey);
    const expiresIn = localStorage.getItem(this.expiresInKey);

    if (!token || !expiresIn) {
      return false;
    }

    const expirationTime = parseInt(expiresIn, 10) * 1000;
    return Date.now() < expirationTime;
  }

  private setupTokenRefresh(): void {
    this.isAuthenticatedSubject.pipe(
      switchMap(isAuthenticated => {
        if (!isAuthenticated) {
          return new Observable(observer => observer.complete());
        }

        const expiresIn = localStorage.getItem(this.expiresInKey);
        if (!expiresIn) {
          return new Observable(observer => observer.complete());
        }

        // Refresh token 5 minutes before expiration
        const refreshDelay = (parseInt(expiresIn, 10) * 1000) - (5 * 60 * 1000);
        return timer(Math.max(0, refreshDelay)).pipe(
          switchMap(() => this.refreshToken())
        );
      })
    ).subscribe();
  }
}
