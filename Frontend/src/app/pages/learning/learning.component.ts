import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-learning',
  templateUrl: './learning.component.html',
  styleUrls: ['./learning.component.scss']
})
export class LearningComponent implements OnInit {
  currentLesson = {
    id: 1,
    title: 'Introduction to HTML',
    course: 'Web Development Fundamentals',
    instructor: 'Sarah Johnson',
    duration: '45:30',
    videoUrl: '#',
    progress: 65
  };

  lessons = [
    { id: 1, title: 'Introduction to HTML', watched: true, duration: '45:30' },
    { id: 2, title: 'HTML Tags and Elements', watched: true, duration: '38:15' },
    { id: 3, title: 'HTML Forms and Input', watched: false, duration: '42:00' },
    { id: 4, title: 'Semantic HTML', watched: false, duration: '35:45' },
    { id: 5, title: 'HTML Best Practices', watched: false, duration: '40:30' }
  ];

  notes = [
    { id: 1, timestamp: '5:30', content: 'Important: HTML is the foundation of web development' },
    { id: 2, timestamp: '12:45', content: 'Remember to always use semantic tags for accessibility' }
  ];

  newNote = '';
  isLessonListOpen = true;

  constructor() { }

  ngOnInit(): void {
  }

  addNote(): void {
    if (this.newNote.trim()) {
      this.notes.push({
        id: this.notes.length + 1,
        timestamp: '15:00',
        content: this.newNote
      });
      this.newNote = '';
    }
  }

  selectLesson(lesson: any): void {
    this.currentLesson = { ...this.currentLesson, ...lesson };
  }

  toggleLessonList(): void {
    this.isLessonListOpen = !this.isLessonListOpen;
  }
}

