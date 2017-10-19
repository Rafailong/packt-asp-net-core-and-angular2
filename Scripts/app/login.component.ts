import { Component } from '@angular/core';
import { FormBuilder, FormGroup, FormControl, Validators } from "@angular/forms";
import { Router } from "@angular/router";

import { AuthService } from "./auth.service";

@Component({
  selector: 'login',
  template: `
  <div class="login-container">
    <h2 class="form-login-heading">Login</h2>
    <div class="alert alert-danger" role="alert" *ngIf="loginError">
      <strong>Warning:</strong> Username or Password mismatch
    </div>
    <form class="form-login" [formGroup]="loginForm" (submit)="performLogin($event)" >

      <div class="form-group"
        [ngClass]="{
          'has-error': username.invalid && (username.dirty || username.touched),
          'has-success': username.valid && (username.dirty || username.touched)
        }">
        <label for="username">eMail</label>
        <input type="text" class="form-control" formControlName="username"
          placeholder="Your username or e-mail address" required />
      </div>

      <div class="form-group"
        [ngClass]="{
          'has-error': password.invalid && (password.dirty || password.touched),
          'has-success': password.valid && (password.dirty || password.touched)
        }">
        <label for="password">Password</label>
        <input type="password" class="form-control" formControlName="password" name="password"
          required />
      </div>

      <button class="btn btn-lg btn-primary btn-block" type="submit" [disabled]="!this.loginForm.valid">Sign in</button>
    </form>
  </div>
  `
})
export class LoginComponent {
  title = "Login";
  loginForm: FormGroup;
  username: FormControl;
  password: FormControl;

  loginError: any = false;

  constructor(
    private router: Router,
    private fb: FormBuilder,
    private authService: AuthService
  ) {
    if (this.authService.isLoggedIn()) {
      this.router.navigate([""]);
    }

    this.username = new FormControl('', Validators.required);
    this.password = new FormControl('', Validators.required);
    this.loginForm = fb.group({
      username: this.username,
      password: this.password
    });
  }
  performLogin(e: any) {
    e.preventDefault();
    const username = this.loginForm.value.username;
    const password = this.loginForm.value.password;
    this.authService.login(username, password).subscribe((data: any) => {
      // login successful
      this.loginError = false;
      var auth = this.authService.getAuth();
      alert("Our Token is: " + auth.access_token);
      this.router.navigate([""]);
    },
    (err: any) => {
      console.log(err);
      // login failure
      this.loginError = true;
    });
  }
}
