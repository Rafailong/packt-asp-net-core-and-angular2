import {Component} from '@angular/core';
import {FormBuilder, FormGroup, Validators} from "@angular/forms";
import {Router} from "@angular/router";

@Component({
  selector: 'login',
  template: `
  <div class="login-container">
    <h2 class="form-login-heading">Login</h2>
    <div class="alert alert-danger" role="alert" *ngIf="loginError">
      <strong>Warning:</strong> Username or Password mismatch
    </div>
    <form class="form-login" [formGroup]="loginForm" (submit)="performLogin($event)" novalidate >
      <input type="text" class="form-control" formControlName="username"
        placeholder="Your username or e-mail address" required autofocus />
      <input type="password" class="form-control" formControlName="password"
        placeholder="Your password" required />
      <div class="checkbox">
        <label>
          <input type="checkbox" value="remember-me" />
          Remember me
        </label>
      </div>
      <button class="btn btn-lg btn-primary btn-block" type="submit">Sign in</button>
    </form>
    <p>Form status: {{ loginForm.status | json }}</p>
  </div>
  `
})
export class LoginComponent {
  title = "Login";
  loginForm: FormGroup;
  loginError: any = null;

  constructor(private fb: FormBuilder) {
    this.loginForm = fb.group({
      username: ["", Validators.required],
      password: ["", Validators.required]
    });
  }
  performLogin(e: any) {
    e.preventDefault();
    alert(JSON.stringify(this.loginForm.value));
  }
}