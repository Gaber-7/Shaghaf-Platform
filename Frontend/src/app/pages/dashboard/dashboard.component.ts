import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent implements OnInit {
  studentName = 'Michael Scott';
  courseCount = 12;
  assignmentCount = 8;
  studyHours = 24;
  certificateCount = 5;
  progressPercentage = 70;

  courses = [
    { name: 'Web Development', progress: 85, lessons: 24 },
    { name: 'Mobile Apps', progress: 65, lessons: 18 },
    { name: 'Data Science', progress: 45, lessons: 20 }
  ];

  constructor() { }

  ngOnInit(): void {
  }
}

