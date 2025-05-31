import { Component, inject, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { TOKEN } from '../shared/constants';
import { AuthService } from '../shared/services/auth.service';
import { UserService } from '../shared/services/user.service';

@Component({
  selector: 'app-dashboard',
  imports: [],
  templateUrl: './dashboard.component.html',
  styles: ``
})
export class DashboardComponent implements OnInit
{
  userService=inject(UserService);
  fullName:string='';

  ngOnInit(): void {
    this.userService.getUserProfile().subscribe({
      next:(res:any)=>{this.fullName=res.fullName},
      error:(err:any)=>{console.log('error while retriving the user profile')}
    });
  }

  
}
