
import { Component, OnInit, inject } from '@angular/core';
import { AuthService } from './Auhentication/auth.service';
import { AuthResponse } from './Auhentication/auth-response';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent
{  
  title = 'NG.ClientProductsAPI';
  private authService = inject(AuthService);
  user!: AuthResponse;
  public login!: boolean;


  ngOnInit() {

    this.login = this.authService.isLoggedIn();
    this.user = this.authService.getCurrentAuthUser();
    
  }

  logout() {
    this.authService.logout();
  }

  refreshToken() {
    this.authService.refreshToken()?.subscribe(() => { });
  }


}
