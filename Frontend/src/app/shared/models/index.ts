// Auth Models
export interface LoginRequest {
  email: string;
  password: string;
}

export interface RegisterRequest {
  firstName: string;
  lastName: string;
  email: string;
  password: string;
  role: UserRole;
}

export interface AuthResponse {
  accessToken: string;
  refreshToken: string;
  expiresIn: number;
  user: UserDto;
}

export interface UserDto {
  id: string;
  firstName: string;
  lastName: string;
  email: string;
  role: UserRole;
  profilePictureUrl?: string;
  createdAt: Date;
}

export interface RefreshTokenRequest {
  refreshToken: string;
}

export interface ChangePasswordRequest {
  currentPassword: string;
  newPassword: string;
}

export type UserRole = 'Student' | 'Teacher' | 'Parent' | 'Admin' | 'SuperAdmin';

// Course Models
export interface CourseListItemDto {
  id: string;
  title: string;
  description: string;
  instructorId: string;
  instructorName: string;
  thumbnailUrl?: string;
  price: number;
  rating: number;
  enrollmentCount: number;
  isPublished: boolean;
  createdAt: Date;
}

export interface CourseDetailDto extends CourseListItemDto {
  content: string;
  sections: SectionDto[];
  tags: string[];
  categoryId: string;
}

export interface SectionDto {
  id: string;
  courseId: string;
  title: string;
  description?: string;
  orderIndex: number;
  lessons: LessonDto[];
}

export interface LessonDto {
  id: string;
  sectionId: string;
  title: string;
  description?: string;
  orderIndex: number;
  videoUrl?: string;
  duration?: number;
  isFree: boolean;
}

export interface CourseQuery {
  searchTerm?: string;
  category?: string;
  level?: string;
  page?: number;
  pageSize?: number;
  sortBy?: string;
  sortOrder?: 'asc' | 'desc';
}

export interface PagedResult<T> {
  items: T[];
  totalCount: number;
  page: number;
  pageSize: number;
  totalPages: number;
}

// Enrollment Models
export interface EnrollmentDto {
  id: string;
  studentId: string;
  courseId: string;
  enrolledAt: Date;
  completedAt?: Date;
  progress: number;
  isActive: boolean;
}

export interface CreateEnrollmentRequest {
  courseId: string;
}

// Lesson Progress Models
export interface LessonProgressDto {
  id: string;
  studentId: string;
  lessonId: string;
  watchedDuration: number;
  totalDuration: number;
  isCompleted: boolean;
  completedAt?: Date;
  lastWatchedAt: Date;
}

// API Response Models
export interface ApiResponse<T> {
  success: boolean;
  message?: string;
  data?: T;
  errors?: string[];
}
