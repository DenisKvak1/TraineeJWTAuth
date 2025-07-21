import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {HomeComponent} from './home/home.component';
import {NotFoundComponent} from './not-found/not-found.component';
import {AuthGuard} from '../auth/isAuth.guard';
import {PrivacyComponent} from './privacy/privacy.component';

const routes: Routes = [
  {
    path: 'auth',
    loadChildren: () => import('../auth/auth.module').then(m => m.AuthModule)
  },
  {path: '', component: HomeComponent},
  {
    path: '',
    canActivate: [AuthGuard],
    children: [
      {path: 'privacy', component: PrivacyComponent},
    ]
  },

  {path: '**', component: NotFoundComponent},
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {
}
