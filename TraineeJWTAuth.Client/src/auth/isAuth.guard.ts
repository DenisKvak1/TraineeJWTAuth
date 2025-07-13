import {AuthService} from './auth.service';
import {inject, Injectable} from '@angular/core';
import {ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot} from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {
  constructor(
    private authService: AuthService,
    private router: Router
  ) {
  }

  canActivate(): boolean {
    if (this.authService.isAuth()) {
      return true;
    } else {
      this.router.navigate(['auth/login']);
      return false;
    }
  }
}
