import { Component } from '@angular/core';
import { RegistrationComponent } from "./registration/registration.component";
import { RouterLink, RouterModule, RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-user',
  imports: [RegistrationComponent,RouterOutlet,RouterLink],
  templateUrl: './user.component.html',
  styles: ``
})
export class UserComponent {

}
