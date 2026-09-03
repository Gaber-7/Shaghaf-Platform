import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-analytics',
  templateUrl: './analytics.component.html',
  styleUrls: ['./analytics.component.scss']
})
export class AnalyticsComponent implements OnInit {
  stats = [
    { label: 'Total Learning Hours', value: '124 hrs', icon: '⏱️', trend: '+12% this month' },
    { label: 'Courses Completed', value: '5', icon: '🎓', trend: '+1 this month' },
    { label: 'Current Streak', value: '12 days', icon: '🔥', trend: '+3 days' },
    { label: 'Average Score', value: '87%', icon: '⭐', trend: '+5% this month' }
  ];

  performanceData = [
    { course: 'Web Development', score: 92, status: 'Excellent' },
    { course: 'Python Basics', score: 78, status: 'Good' },
    { course: 'UI/UX Design', score: 85, status: 'Good' },
    { course: 'Mobile Apps', score: 88, status: 'Excellent' },
    { course: 'Data Science', score: 72, status: 'Fair' }
  ];

  timeData = [
    { day: 'Mon', hours: 2 },
    { day: 'Tue', hours: 3 },
    { day: 'Wed', hours: 1 },
    { day: 'Thu', hours: 4 },
    { day: 'Fri', hours: 2 },
    { day: 'Sat', hours: 5 },
    { day: 'Sun', hours: 3 }
  ];

  skillsData = [
    { skill: 'JavaScript', level: 85 },
    { skill: 'HTML/CSS', level: 92 },
    { skill: 'React', level: 78 },
    { skill: 'Python', level: 72 },
    { skill: 'UI/UX', level: 88 }
  ];

  constructor() { }

  ngOnInit(): void {
  }

  getStatusColor(status: string): string {
    switch (status) {
      case 'Excellent': return '#A8E063';
      case 'Good': return '#A8E063';
      case 'Fair': return '#FFA500';
      default: return '#999';
    }
  }
}

