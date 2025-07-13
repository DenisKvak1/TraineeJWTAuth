import {
  HttpEvent,
  HttpHandler,
  HttpHandlerFn,
  HttpInterceptor,
  HttpInterceptorFn,
  HttpRequest
} from '@angular/common/http';
import {inject, Injectable} from '@angular/core';
import {AuthService} from './auth.service';
import {catchError, finalize, Observable, switchMap, throwError} from 'rxjs';


@Injectable()
export class AuthInterceptor implements HttpInterceptor {
  private isRefreshing = false;

  constructor(
    private authService: AuthService
  ) {
  }


  intercept(req: HttpRequest<any>, next: HttpHandler) {
    const token = this.authService.accessToken;
    if (!token) return next.handle(req)

    if (this.isRefreshing) return next.handle(req)

    return next.handle(this.addToken(token, req))
      .pipe(
        catchError((error) => {
          if (error.status !== 401) return throwError(error);

          return this.refreshToken(req, next)
        })
      )
  }

  private refreshToken(req: HttpRequest<any>, next: HttpHandler) {
    if (!this.isRefreshing) this.isRefreshing = true;
    return this.authService.refresh()
      .pipe(
        finalize(() => this.isRefreshing = false),
        switchMap((res) => {
          return next.handle(this.addToken(res.data.accessToken, req))
        })
      )
  }

  private addToken(token: string, req: HttpRequest<any>) {
    return req.clone({
      setHeaders: {
        'Authorization': `Bearer ${token}`
      }
    })
  }
}
