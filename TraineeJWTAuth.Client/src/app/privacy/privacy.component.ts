import {Component} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {environment} from '../../environment';
import {ServerResponse} from '../../common/ServerResponse';

@Component({
  selector: 'app-privacy',
  standalone: false,
  templateUrl: './privacy.component.html',
  styleUrl: './privacy.component.css'
})
export class PrivacyComponent {
  public privateMsg: string | null = null;

  constructor(
    private httpClient: HttpClient
  ) {
    httpClient.get<ServerResponse>(`${environment.apiBaseUrl}/home/privacy`)
      .subscribe((value) => {
        this.privateMsg = value.data;
      })
  }
}
