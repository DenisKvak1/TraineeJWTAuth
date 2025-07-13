import {Component, OnInit} from '@angular/core';
import {AuthService} from '../auth.service';
import {FormControl, FormGroup, Validators} from '@angular/forms';
import {LoginForm} from '../models/LoginForm';
import {Router} from '@angular/router';

@Component({
  selector: 'app-login',
  standalone: false,
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent implements OnInit {
  form = new FormGroup({
    email: new FormControl<string>('', [Validators.required, Validators.email]),
    password: new FormControl<string>('', [Validators.required]),
    rememberMe: new FormControl<boolean>(false)
  })
  serverErrors: string[] = []

  constructor(
    private authService: AuthService,
    private router: Router
  ) {
  }

  public onSubmit() {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }
    this.authService.login(this.form.value as LoginForm)
      .subscribe({
        next: () => {
          this.router.navigate([''])
        },
        error: err => {
          this.serverErrors = err?.error?.errors
        },
      });
  }

  public ngOnInit(): void {
    console.log(this.authService.isAuth())
    if (this.authService.isAuth()) {
      this.router.navigate([''])
    }
  }
}
