import { Component, OnInit } from '@angular/core';
import { ApiService } from '@app/core/services/api.service';
import { CourseListItemDto, CourseQuery, PagedResult } from '@app/shared/models';

@Component({
  selector: 'app-courses',
  templateUrl: './courses.component.html',
  styleUrls: ['./courses.component.scss']
})
export class CoursesComponent implements OnInit {
  searchQuery = '';
  selectedCategory = 'all';
  selectedLevel = 'all';
  isLoading = true;
  errorMessage = '';

  courses: CourseListItemDto[] = [];
  filteredCoursesList: CourseListItemDto[] = [];

  categories = [
    { value: 'all', label: 'All Courses' },
    { value: 'web', label: 'Web Development' },
    { value: 'data', label: 'Data Science' },
    { value: 'design', label: 'Design' },
    { value: 'mobile', label: 'Mobile' }
  ];

  levels = [
    { value: 'all', label: 'All Levels' },
    { value: 'beginner', label: 'Beginner' },
    { value: 'intermediate', label: 'Intermediate' },
    { value: 'advanced', label: 'Advanced' }
  ];

  constructor(private apiService: ApiService) { }

  ngOnInit(): void {
    this.loadCourses();
  }

  private loadCourses(): void {
    const query: CourseQuery = {
      page: 1,
      pageSize: 20,
      sortBy: 'createdAt',
      sortOrder: 'desc'
    };

    this.apiService.searchCourses(query).subscribe({
      next: (result: PagedResult<CourseListItemDto>) => {
        this.courses = result.items;
        this.updateFilteredCourses();
        this.isLoading = false;
      },
      error: (error) => {
        console.error('Error loading courses:', error);
        this.errorMessage = 'Failed to load courses';
        this.isLoading = false;
      }
    });
  }

  private updateFilteredCourses(): void {
    this.filteredCoursesList = this.courses.filter(course => {
      const matchesSearch = course.title.toLowerCase().includes(this.searchQuery.toLowerCase()) ||
                           course.instructorName.toLowerCase().includes(this.searchQuery.toLowerCase());
      // Add more filtering logic based on category and level if needed
      return matchesSearch;
    });
  }

  onSearchChange(): void {
    this.updateFilteredCourses();
  }

  onCategoryChange(): void {
    this.updateFilteredCourses();
  }

  onLevelChange(): void {
    this.updateFilteredCourses();
  }

  get filteredCourses(): CourseListItemDto[] {
    return this.filteredCoursesList;
  }
}

