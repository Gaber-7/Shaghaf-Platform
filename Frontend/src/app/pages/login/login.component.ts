import { Component } from '@angular/core';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent {
  email: string = '';
  password: string = '';
  rememberMe: boolean = false;
  activeTab: string = 'student';

  onSignIn() {
    // Handle sign-in logic
    console.log('Sign in:', this.email, this.password);
  }

  onSignUp() {
    // Handle sign-up navigation
    console.log('Navigate to sign up');
  }
}
