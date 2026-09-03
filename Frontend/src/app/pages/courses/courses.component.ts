import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-courses',
  templateUrl: './courses.component.html',
  styleUrls: ['./courses.component.scss']
})
export class CoursesComponent implements OnInit {
  searchQuery = '';
  selectedCategory = 'all';
  selectedLevel = 'all';

  courses = [
    {
      id: 1,
      name: 'Web Development Fundamentals',
      instructor: 'Sarah Johnson',
      category: 'web',
      level: 'beginner',
      students: 1250,
      rating: 4.8,
      image: '📚',
      description: 'Learn HTML, CSS, and JavaScript basics',
      price: 'Free'
    },
    {
      id: 2,
      name: 'React Advanced Patterns',
      instructor: 'John Smith',
      category: 'web',
      level: 'advanced',
      students: 890,
      rating: 4.9,
      image: '⚛️',
      description: 'Master advanced React concepts and patterns',
      price: '$49'
    },
    {
      id: 3,
      name: 'Python for Data Science',
      instructor: 'Emma Davis',
      category: 'data',
      level: 'intermediate',
      students: 2150,
      rating: 4.7,
      image: '🐍',
      description: 'Learn Python with Pandas, NumPy, and Matplotlib',
      price: '$39'
    },
    {
      id: 4,
      name: 'UI/UX Design Essentials',
      instructor: 'Michael Chen',
      category: 'design',
      level: 'beginner',
      students: 1500,
      rating: 4.6,
      image: '🎨',
      description: 'Create beautiful and functional user interfaces',
      price: 'Free'
    },
    {
      id: 5,
      name: 'Machine Learning Basics',
      instructor: 'Dr. Aisha Patel',
      category: 'data',
      level: 'advanced',
      students: 680,
      rating: 4.9,
      image: '🤖',
      description: 'Introduction to ML algorithms and implementation',
      price: '$79'
    },
    {
      id: 6,
      name: 'Mobile App Development',
      instructor: 'James Wilson',
      category: 'mobile',
      level: 'intermediate',
      students: 950,
      rating: 4.5,
      image: '📱',
      description: 'Build iOS and Android apps with React Native',
      price: '$59'
    }
  ];

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

  constructor() { }

  ngOnInit(): void {
  }

  get filteredCourses() {
    return this.courses.filter(course => {
      const matchesSearch = course.name.toLowerCase().includes(this.searchQuery.toLowerCase()) ||
                           course.instructor.toLowerCase().includes(this.searchQuery.toLowerCase());
      const matchesCategory = this.selectedCategory === 'all' || course.category === this.selectedCategory;
      const matchesLevel = this.selectedLevel === 'all' || course.level === this.selectedLevel;
      return matchesSearch && matchesCategory && matchesLevel;
    });
  }
}

