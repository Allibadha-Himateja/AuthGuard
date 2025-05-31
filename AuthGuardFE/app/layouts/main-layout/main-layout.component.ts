import { Component,inject } from '@angular/core';
import { Router, RouterLink, RouterOutlet } from '@angular/router';
import { AuthService } from '../../shared/services/auth.service';

@Component({
  selector: 'app-main-layout',
  imports: [RouterOutlet,RouterLink],
  templateUrl: './main-layout.component.html',
  styles: ``
})
export class MainLayoutComponent {
  service=inject(AuthService);
  router=inject(Router);
  LogOut()
  {
    this.service.deletetoken();
    this.router.navigateByUrl("SignIn")
  }
}
