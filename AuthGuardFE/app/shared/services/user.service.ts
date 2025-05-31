import { HttpClient, HttpHeaders } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../../environments/environment.development';
import { AuthService } from './auth.service';

@Injectable({
  providedIn: 'root'
})
export class UserService 
{

  // constructor(private http:HttpClient,private userService:UserService,private authService:AuthService) { }
  http=inject(HttpClient);
  authService=inject(AuthService);

  getUserProfile()
  {
    // const reqHeader=new HttpHeaders({'Authorization':'Bearer'+this.authService.getToken()}),{headers:reqHeader}
    return this.http.get(environment.apiBaseUrl+'/UserProfile');
  }
}

