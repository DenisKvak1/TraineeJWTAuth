import {Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {LoginForm} from './models/LoginForm';
import {environment} from '../environment';
import {catchError, of, switchMap, tap, throwError} from 'rxjs';
import {ServerResponse} from '../common/ServerResponse';
import {RegisterForm} from './models/RegisterForm';
import {LoginResponse} from './responces/LoginResponse';
import {RegisterResponse} from './responces/RegisterResponce';
import {RefreshTokenResponse} from './responces/RefreshTokenResponce';
import {Router} from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private baseUrl = `${environment.apiBaseUrl}/Account`;
  accessToken: string | null = null;

  constructor(
    private httpClient: HttpClient,
    private router: Router
  ) {
    this.loadAccessToken()
  }

  public login(loginForm: LoginForm) {
    return this.httpClient.post<ServerResponse<LoginResponse>>(`${this.baseUrl}/login`, loginForm, {
      withCredentials: true
    })
      .pipe(
        tap((response) => {
          this.saveAccessToken(response.data.accessToken)
        }))
  }

  public register(registerForm: RegisterForm) {
    return this.httpClient.post<ServerResponse<RegisterResponse>>(`${this.baseUrl}/register`, registerForm, {
      withCredentials: true
    })
      .pipe(
        tap((response) => {
          this.saveAccessToken(response.data.accessToken)
        })
      )
  }

  public refresh() {
    return this.httpClient.get<ServerResponse<RefreshTokenResponse>>(`${this.baseUrl}/Refresh`, {
      withCredentials: true
    })
      .pipe(
        tap((response) => {
          this.saveAccessToken(response.data.accessToken)
        }),
        catchError((err) => {
          this.logout()
          return throwError(err)
        })
      )
  }

  public logout() {
    this.saveAccessToken('')
    this.router.navigate(['auth/login']);
  }

  public isAuth() {
    if (!this.accessToken) {
      this.loadAccessToken()
    }
    return !!this.accessToken;
  }

  private loadAccessToken() {
    this.accessToken = localStorage.getItem('access_token');
  }

  private saveAccessToken(accessToken: string) {
    localStorage.setItem('access_token', accessToken);
    this.accessToken = accessToken;
  }
}
