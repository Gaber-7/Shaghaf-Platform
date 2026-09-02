import { Component } from '@angular/core';

@Component({
  selector: 'app-root',
  template: `
    <app-topbar></app-topbar>
    <div class="app-layout">
      <app-sidebar></app-sidebar>
      <main class="main-content">
        <router-outlet></router-outlet>
      </main>
    </div>
  `,
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  title = 'frontend';
}
